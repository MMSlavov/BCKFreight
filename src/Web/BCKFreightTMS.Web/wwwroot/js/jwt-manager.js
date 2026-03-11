/**
 * JWT Token Manager
 * Handles JWT token storage, retrieval, validation, and refresh
 */
class JwtTokenManager {
    constructor() {
        this.tokenKey = 'bck_jwt_token';
        this.tokenExpiryKey = 'bck_jwt_expiry';
        this.userKey = 'bck_user_info';
        this.useApiKey = 'bck_use_api';
    }

    /**
     * Store JWT token and user information
     * @param {string} token - JWT token
     * @param {Date|string} expiration - Token expiration date
     * @param {object} userInfo - User information (userId, username, etc.)
     */
    setToken(token, expiration, userInfo = null) {
        try {
            localStorage.setItem(this.tokenKey, token);
            
            const expiryDate = typeof expiration === 'string' ? new Date(expiration) : expiration;
            localStorage.setItem(this.tokenExpiryKey, expiryDate.toISOString());
            
            if (userInfo) {
                localStorage.setItem(this.userKey, JSON.stringify(userInfo));
            }
            
            console.log('JWT token stored successfully');
            return true;
        } catch (error) {
            console.error('Error storing JWT token:', error);
            return false;
        }
    }

    /**
     * Get the stored JWT token
     * @returns {string|null} JWT token or null if not found/expired
     */
    getToken() {
        try {
            const token = localStorage.getItem(this.tokenKey);
            
            if (!token) {
                return null;
            }
            
            if (this.isTokenExpired()) {
                this.clearToken();
                return null;
            }
            
            return token;
        } catch (error) {
            console.error('Error retrieving JWT token:', error);
            return null;
        }
    }

    /**
     * Check if the token is expired
     * @returns {boolean} True if token is expired or about to expire
     */
    isTokenExpired() {
        try {
            const expiryStr = localStorage.getItem(this.tokenExpiryKey);
            
            if (!expiryStr) {
                return true;
            }
            
            const expiry = new Date(expiryStr);
            const now = new Date();
            
            // Add 1 minute buffer to prevent edge cases
            const bufferMs = 60 * 1000;
            return (expiry.getTime() - now.getTime()) < bufferMs;
        } catch (error) {
            console.error('Error checking token expiration:', error);
            return true;
        }
    }

    /**
     * Get user information
     * @returns {object|null} User information or null
     */
    getUserInfo() {
        try {
            const userInfoStr = localStorage.getItem(this.userKey);
            return userInfoStr ? JSON.parse(userInfoStr) : null;
        } catch (error) {
            console.error('Error retrieving user info:', error);
            return null;
        }
    }

    /**
     * Clear all stored token data
     */
    clearToken() {
        try {
            localStorage.removeItem(this.tokenKey);
            localStorage.removeItem(this.tokenExpiryKey);
            localStorage.removeItem(this.userKey);
            console.log('JWT token cleared');
        } catch (error) {
            console.error('Error clearing JWT token:', error);
        }
    }

    /**
     * Check if user has a valid token (is authenticated)
     * @returns {boolean} True if user has valid token
     */
    isAuthenticated() {
        const token = this.getToken();
        return token !== null && !this.isTokenExpired();
    }

    /**
     * Get Authorization header value
     * @returns {string|null} Bearer token string or null
     */
    getAuthHeader() {
        const token = this.getToken();
        return token ? `Bearer ${token}` : null;
    }

    /**
     * Check if API mode is enabled
     * @returns {boolean} Always returns true (JWT API is always enabled)
     */
    isApiModeEnabled() {
        return true; // JWT API is now the only authentication mode
    }

    /**
     * Parse JWT token to get claims (without verification)
     * @param {string} token - JWT token
     * @returns {object|null} Decoded token payload
     */
    parseToken(token) {
        try {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));

            return JSON.parse(jsonPayload);
        } catch (error) {
            console.error('Error parsing JWT token:', error);
            return null;
        }
    }

    /**
     * Get token expiration time remaining in seconds
     * @returns {number} Seconds until expiration, or 0 if expired
     */
    getTimeUntilExpiry() {
        try {
            const expiryStr = localStorage.getItem(this.tokenExpiryKey);
            if (!expiryStr) return 0;
            
            const expiry = new Date(expiryStr);
            const now = new Date();
            const diff = Math.floor((expiry.getTime() - now.getTime()) / 1000);
            
            return diff > 0 ? diff : 0;
        } catch (error) {
            return 0;
        }
    }
}

// Create global instance
window.jwtManager = new JwtTokenManager();
