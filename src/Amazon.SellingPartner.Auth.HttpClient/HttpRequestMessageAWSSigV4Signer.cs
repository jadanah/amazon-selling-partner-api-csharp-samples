using System;
using System.Net.Http;
using System.Text;
using Amazon.SellingPartner.Auth.Core;

namespace Amazon.SellingPartner.Auth.HttpClient
{
    public class HttpRequestMessageAWSSigV4Signer
    {
        private AwsAuthenticationCredentials awsCredentials;

        /// <summary>
        /// Constructor for AWSSigV4Signer
        /// </summary>
        /// <param name="awsAuthenticationCredentials">AWS Developer Account Credentials</param>
        public HttpRequestMessageAWSSigV4Signer(AwsAuthenticationCredentials awsAuthenticationCredentials)
        {
            awsCredentials = awsAuthenticationCredentials;
            HttpRequestMessageAwsSignerHelper = new HttpRequestMessageAWSSignerHelper();
        }

        public virtual HttpRequestMessageAWSSignerHelper HttpRequestMessageAwsSignerHelper { get; set; }

        /// <summary>
        /// Signs a Request with AWS Signature Version 4
        /// </summary>
        /// <param name="request">RestRequest which needs to be signed</param>
        /// <param name="host">Request endpoint</param>
        /// <returns>RestRequest with AWS Signature</returns>
        public HttpRequestMessage Sign(HttpRequestMessage request, string host)
        {
            DateTime signingDate = HttpRequestMessageAwsSignerHelper.InitializeHeaders(request, host);
            string signedHeaders = HttpRequestMessageAwsSignerHelper.ExtractSignedHeaders(request);

            string hashedCanonicalRequest = CreateCanonicalRequest(request, signedHeaders);

            string stringToSign = HttpRequestMessageAwsSignerHelper.BuildStringToSign(signingDate,
                hashedCanonicalRequest,
                awsCredentials.Region);

            string signature = HttpRequestMessageAwsSignerHelper.CalculateSignature(stringToSign,
                signingDate,
                awsCredentials.SecretKey,
                awsCredentials.Region);

            HttpRequestMessageAwsSignerHelper.AddSignature(request,
                awsCredentials.AccessKeyId,
                signedHeaders,
                signature,
                awsCredentials.Region,
                signingDate);

            return request;
        }

        private string CreateCanonicalRequest(HttpRequestMessage request, string signedHeaders)
        {
            var canonicalizedRequest = new StringBuilder();
            //Request Method
            canonicalizedRequest.AppendFormat("{0}\n", request.Method);

            //CanonicalURI
            canonicalizedRequest.AppendFormat("{0}\n", HttpRequestMessageAwsSignerHelper.ExtractCanonicalURIParameters(request.RequestUri.AbsolutePath));

            //CanonicalQueryString
            canonicalizedRequest.AppendFormat("{0}\n", HttpRequestMessageAwsSignerHelper.ExtractCanonicalQueryString(request));

            //CanonicalHeaders
            canonicalizedRequest.AppendFormat("{0}\n", HttpRequestMessageAwsSignerHelper.ExtractCanonicalHeaders(request));

            //SignedHeaders
            canonicalizedRequest.AppendFormat("{0}\n", signedHeaders);

            // Hash(digest) the payload in the body
            canonicalizedRequest.AppendFormat(HttpRequestMessageAwsSignerHelper.HashRequestBody(request));

            string canonicalRequest = canonicalizedRequest.ToString();

            //Create a digest(hash) of the canonical request
            return Utils.ToHex(Utils.Hash(canonicalRequest));
        }
    }
}