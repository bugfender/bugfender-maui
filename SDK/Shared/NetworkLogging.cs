using System.Collections.Generic;

namespace Bugfender.Sdk
{
    /// <summary>
    /// Obfuscated request fields returned by a request obfuscation handler.
    /// </summary>
    public sealed class NetworkRequestData
    {
        public NetworkRequestData(string url, IDictionary<string, string> headers, string? body)
        {
            Url = url ?? "";
            Headers = headers ?? new Dictionary<string, string>();
            Body = body;
        }

        public string Url { get; }
        public IDictionary<string, string> Headers { get; }
        public string? Body { get; }
    }

    /// <summary>
    /// Obfuscated response fields returned by a response obfuscation handler.
    /// </summary>
    public sealed class NetworkResponseData
    {
        public NetworkResponseData(IDictionary<string, string> headers, string? body)
        {
            Headers = headers ?? new Dictionary<string, string>();
            Body = body;
        }

        public IDictionary<string, string> Headers { get; }
        public string? Body { get; }
    }

    /// <summary>
    /// Request obfuscation: receive URL, headers, and body; return possibly redacted values.
    /// </summary>
    public delegate NetworkRequestData NetworkLoggingRequestObfuscationHandler(
        string url,
        IDictionary<string, string> headers,
        string? body);

    /// <summary>
    /// Response obfuscation: receive headers and body; return possibly redacted values.
    /// </summary>
    public delegate NetworkResponseData NetworkLoggingResponseObfuscationHandler(
        IDictionary<string, string> headers,
        string? body);

    /// <summary>
    /// Result of <see cref="IBugfenderBinding.SendInstrumentedNetworkRequest"/>.
    /// Used by samples to verify <c>bf_network</c> capture (OkHttp on Android, URLSession on iOS).
    /// </summary>
    public sealed class InstrumentedNetworkResult
    {
        public InstrumentedNetworkResult(int status, bool shouldCapture, string? requestId)
        {
            Status = status;
            ShouldCapture = shouldCapture;
            RequestId = requestId;
        }

        public int Status { get; }
        public bool ShouldCapture { get; }
        public string? RequestId { get; }
    }
}
