package com.bugfender.sdk.maui;

import android.util.Log;

import com.bugfender.sdk.BugfenderOkHttpEventListenerFactory;
import com.bugfender.sdk.BugfenderOkHttpInterceptor;

import java.lang.reflect.Method;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.TimeUnit;

import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

/**
 * Example / verification helper: HTTP via OkHttp + Bugfender interceptor so traffic
 * appears as {@code bf_network}. .NET HttpClient is not OkHttp and is not observed.
 *
 * Bugfender SDK calls use reflection so this file can compile in the binding project
 * (android-sdk.aar is not on the AndroidJavaSource javac classpath).
 */
public final class InstrumentedNetworkHelper {
    private static final String TAG = "BF/MauiNet";

    private InstrumentedNetworkHelper() {}

    public static Map<String, Object> send(
            String requestUrl,
            String httpMethod,
            String body,
            Map<String, String> extraHeaders
    ) throws Exception {
        if (requestUrl == null || requestUrl.isEmpty()) {
            requestUrl = "https://example.com/";
        }
        String method = (httpMethod == null || httpMethod.isEmpty())
                ? "GET"
                : httpMethod.toUpperCase();
        if (extraHeaders == null) {
            extraHeaders = new HashMap<>();
        }

        boolean shouldCapture = shouldCaptureNetworkRequest(requestUrl);
        Log.i(TAG, "shouldCapture=" + shouldCapture + " method=" + method + " url=" + requestUrl);

        OkHttpClient client = new OkHttpClient.Builder()
                .connectTimeout(15, TimeUnit.SECONDS)
                .readTimeout(15, TimeUnit.SECONDS)
                .writeTimeout(15, TimeUnit.SECONDS)
                .addInterceptor(new BugfenderOkHttpInterceptor())
                .eventListenerFactory(new BugfenderOkHttpEventListenerFactory())
                .build();

        Request.Builder requestBuilder = new Request.Builder().url(requestUrl);
        if (!extraHeaders.containsKey("Authorization")
                && !extraHeaders.containsKey("authorization")) {
            requestBuilder.header("Authorization", "secret-token");
        }
        for (Map.Entry<String, String> header : extraHeaders.entrySet()) {
            if (header.getKey() != null && header.getValue() != null) {
                requestBuilder.header(header.getKey(), header.getValue());
            }
        }
        if ("POST".equals(method) || "PUT".equals(method) || "PATCH".equals(method)) {
            RequestBody requestBody = RequestBody.create(
                    body != null ? body : "{}",
                    MediaType.parse("application/json; charset=utf-8")
            );
            requestBuilder.method(method, requestBody);
        } else {
            requestBuilder.method(method, null);
        }

        try (Response response = client.newCall(requestBuilder.build()).execute()) {
            final int code = response.code();
            String reqId = response.request().header("X-Bugfender-Request-ID");
            Log.i(TAG, "okhttp status=" + code + " reqId=" + reqId);
            debugLog(
                    "okhttp status=" + code
                            + " shouldCapture=" + shouldCapture
                            + " url=" + requestUrl
                            + " reqId=" + reqId
            );
            forceSendOnce();

            Map<String, Object> payload = new HashMap<>();
            payload.put("status", Integer.valueOf(code));
            payload.put("shouldCapture", Boolean.valueOf(shouldCapture));
            payload.put("requestId", reqId);
            return payload;
        }
    }

    private static boolean shouldCaptureNetworkRequest(String requestUrl) {
        try {
            Class<?> bugfender = Class.forName("com.bugfender.sdk.Bugfender");
            Method captureMethod = bugfender.getDeclaredMethod(
                    "shouldCaptureNetworkRequestInternal",
                    String.class
            );
            captureMethod.setAccessible(true);
            Object value = captureMethod.invoke(null, requestUrl);
            return value instanceof Boolean && (Boolean) value;
        } catch (Exception ignored) {
            return false;
        }
    }

    private static void debugLog(String message) {
        try {
            Class<?> bugfender = Class.forName("com.bugfender.sdk.Bugfender");
            Method d = bugfender.getMethod("d", String.class, String.class);
            d.invoke(null, "bf_maui_debug", message);
        } catch (Exception ignored) {
            Log.d(TAG, message);
        }
    }

    private static void forceSendOnce() {
        try {
            Class<?> bugfender = Class.forName("com.bugfender.sdk.Bugfender");
            Method force = bugfender.getMethod("forceSendOnce");
            force.invoke(null);
        } catch (Exception ignored) {
            // ignore
        }
    }
}
