using System;
using System.Text;
using Amazon.SellingPartner.Auth.Core;
using RestSharp;

namespace Amazon.SellingPartner.Auth.RestSharp
{
    public class RestSharpAwsSigV4Signer
    {
        private AwsAuthenticationCredentials awsCredentials;

        /// <summary>
        /// Constructor for RestSharpAwsSigV4Signer
        /// </summary>
        /// <param name="awsAuthenticationCredentials">AWS Developer Account Credentials</param>
        public RestSharpAwsSigV4Signer(AwsAuthenticationCredentials awsAuthenticationCredentials)
        {
            awsCredentials = awsAuthenticationCredentials;
            RestSharpAwsSignerHelper = new RestSharpAwsSignerHelper();
        }

        public virtual RestSharpAwsSignerHelper RestSharpAwsSignerHelper { get; set; }

        /// <summary>
        /// Signs a Request with AWS Signature Version 4
        /// </summary>
        /// <param name="request">RestRequest which needs to be signed</param>
        /// <param name="host">Request endpoint</param>
        /// <returns>RestRequest with AWS Signature</returns>
        public IRestRequest Sign(IRestRequest request, string host)
        {
            DateTime signingDate = RestSharpAwsSignerHelper.InitializeHeaders(request, host);
            string signedHeaders = RestSharpAwsSignerHelper.ExtractSignedHeaders(request);

            string hashedCanonicalRequest = CreateCanonicalRequest(request, signedHeaders);

            string stringToSign = RestSharpAwsSignerHelper.BuildStringToSign(signingDate,
                hashedCanonicalRequest,
                awsCredentials.Region);

            string signature = RestSharpAwsSignerHelper.CalculateSignature(stringToSign,
                signingDate,
                awsCredentials.SecretKey,
                awsCredentials.Region);

            RestSharpAwsSignerHelper.AddSignature(request,
                awsCredentials.AccessKeyId,
                signedHeaders,
                signature,
                awsCredentials.Region,
                signingDate);

            return request;
        }

        private string CreateCanonicalRequest(IRestRequest restRequest, string signedHeaders)
        {
            var canonicalizedRequest = new StringBuilder();
            //Request Method
            canonicalizedRequest.AppendFormat("{0}\n", restRequest.Method);

            //CanonicalURI
            canonicalizedRequest.AppendFormat("{0}\n", RestSharpAwsSignerHelper.ExtractCanonicalURIParameters(restRequest.Resource));

            //CanonicalQueryString
            canonicalizedRequest.AppendFormat("{0}\n", RestSharpAwsSignerHelper.ExtractCanonicalQueryString(restRequest));

            //CanonicalHeaders
            canonicalizedRequest.AppendFormat("{0}\n", RestSharpAwsSignerHelper.ExtractCanonicalHeaders(restRequest));

            //SignedHeaders
            canonicalizedRequest.AppendFormat("{0}\n", signedHeaders);

            // Hash(digest) the payload in the body
            canonicalizedRequest.AppendFormat(RestSharpAwsSignerHelper.HashRequestBody(restRequest));

            string canonicalRequest = canonicalizedRequest.ToString();

            //Create a digest(hash) of the canonical request
            return Utils.ToHex(Utils.Hash(canonicalRequest));
        }
    }
}