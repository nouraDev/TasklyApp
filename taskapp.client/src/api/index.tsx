import axios from "axios";

const baseURL =
  window.location.protocol === "https:"
    ? import.meta.env.VITE_API_URL_HTTPS
    : import.meta.env.VITE_API_URL_HTTP;

if (!baseURL) {
  throw new Error("API base URL is not defined in .env");
}

export const api = axios.create({
  baseURL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Global response interceptor
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      const { status } = error.response;
      if (status === 401) console.error("Authentication error: Please log in");
      if (status === 500) console.error("Server error: Please try again later");
      if (status === 400) console.error("Bad request: Please check your input");
    } else {
      console.error("Network or other error", error);
    }
    return Promise.reject(error);
  }
);
