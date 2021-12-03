using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ess;
using Pkcs11Interop.TokenRSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ETA.InvoiceServices.Services
{
    public class SigningServices
    {
        private static readonly string DllLibPath = "eps2003csp11.dll";

        private static string TokenPin = "23278181";

        /// <summary>
        /// Set Token PIN
        /// </summary>
        /// <param name="tokenPin">New Token PIN</param>
        public static void SetTokenPin(string tokenPin)
        {
            TokenPin = tokenPin;
        }

        /// <summary>
        /// Sign byte data with cert in usb Token
        /// </summary>
        /// <param name="data">data to be signed</param>
        /// <returns>return signed data as base64 string</returns>
        public static string SignWithCMS(byte[] data)
        {
            Pkcs11InteropFactories factories = new();
            using IPkcs11Library pkcs11Library = factories.Pkcs11LibraryFactory
                .LoadPkcs11Library(factories, DllLibPath, AppType.MultiThreaded);

            ISlot slot = pkcs11Library.GetSlotList(SlotsType.WithTokenPresent).FirstOrDefault();

            if (slot is null)
            {
                return "No slots found";
            }

            var token = slot.GetTokenInfo();
            var subfi = slot.GetSlotInfo();

            using var session = slot.OpenSession(SessionType.ReadWrite);

            session.Login(CKU.CKU_USER, Encoding.UTF8.GetBytes(TokenPin));

            var searchAttribute = new List<IObjectAttribute>()
                    {
                        session.Factories.ObjectAttributeFactory
                        .Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE),
                        session.Factories.ObjectAttributeFactory
                        .Create(CKA.CKA_TOKEN, true),
                        session.Factories.ObjectAttributeFactory
                        .Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509)
                    };

            IObjectHandle certificate = session.FindAllObjects(searchAttribute).FirstOrDefault();


            if (certificate is null)
            {
                return "Certificate not found";
            }

            var attributeValues = session.GetAttributeValue(certificate, new List<CKA>
                    {
                        CKA.CKA_VALUE
                    });


            var xcert = new X509Certificate2(attributeValues[0].GetValueAsByteArray());

            searchAttribute = new List<IObjectAttribute>()
                    {
                        session.Factories.ObjectAttributeFactory
                        .Create(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY),
                        session.Factories.ObjectAttributeFactory
                        .Create(CKA.CKA_KEY_TYPE,CKK.CKK_RSA)
                    };

            IObjectHandle privateKeyHandler = session.FindAllObjects(searchAttribute).FirstOrDefault();

            RSA privateKey = new TokenRSA(xcert, session, slot, privateKeyHandler);

            ContentInfo content = new(new Oid("1.2.840.113549.1.7.5"), data);


            SignedCms cms = new(content, true);


            EssCertIDv2 bouncyCertificate = new
                (new Org.BouncyCastle.Asn1.X509
                .AlgorithmIdentifier(new DerObjectIdentifier("1.2.840.113549.1.9.16.2.47")),
                HashBytes(xcert.RawData));

            SigningCertificateV2 signerCertificateV2 = new(new EssCertIDv2[] { bouncyCertificate });


            CmsSigner signer = new(xcert)
            {
                PrivateKey = privateKey,

                DigestAlgorithm = new Oid("2.16.840.1.101.3.4.2.1")
            };



            signer.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.UtcNow));
            signer.SignedAttributes.Add(new AsnEncodedData(
                new Oid("1.2.840.113549.1.9.16.2.47"), signerCertificateV2.GetEncoded()));

            cms.ComputeSignature(signer);

            var output = cms.Encode();

            return Convert.ToBase64String(output);

        }

        /// <summary>
        /// Sign byte data with cert in usb Token asynchronous
        /// </summary>
        /// <param name="data">data to be signed</param>
        /// <returns>return signed data as base64 string</returns>
        public async static Task<string> SignWithCMSAsync(byte[] data)
        {
            return await Task.Run(() =>
            {
                return SignWithCMS(data);
            });
        }

        private static byte[] HashBytes(byte[] input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var output = sha.ComputeHash(input);
                return output;
            }
        }
    }
}
