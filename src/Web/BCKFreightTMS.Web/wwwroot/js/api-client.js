/**
 * API Client
 * Centralized API communication with JWT authentication
 */
class ApiClient {
    constructor(baseUrl = '/api') {
        this.baseUrl = baseUrl;
        this.tokenManager = window.jwtManager;
    }

    /**
     * Make an API request
     * @param {string} endpoint - API endpoint (e.g., '/contacts')
     * @param {object} options - Fetch options (method, body, headers, etc.)
     * @returns {Promise<object>} Response data
     */
    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}${endpoint}`;
        
        // Set default headers
        const headers = {
            'Content-Type': 'application/json',
            ...options.headers
        };

        // Add JWT token if available
        const authHeader = this.tokenManager.getAuthHeader();
        if (authHeader) {
            headers['Authorization'] = authHeader;
        }

        const config = {
            ...options,
            headers
        };

        try {
            const response = await fetch(url, config);

            // Handle 401 Unauthorized - token expired or invalid
            if (response.status === 401) {
                this.handleUnauthorized();
                throw new Error('Unauthorized - please login again');
            }

            // Handle 403 Forbidden - insufficient permissions
            if (response.status === 403) {
                throw new Error('Forbidden - insufficient permissions');
            }

            // Check if response is ok
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.error || errorData.message || `API Error: ${response.status}`);
            }

            // Parse JSON response
            const data = await response.json();
            return data;

        } catch (error) {
            console.error('API Request Error:', error);
            throw error;
        }
    }

    /**
     * Handle unauthorized response
     */
    handleUnauthorized() {
        console.warn('Token expired or invalid, clearing token');
        this.tokenManager.clearToken();
        
        // Show notification
        if (typeof notyf !== 'undefined') {
            notyf.error('Your session has expired. Please login again.');
        }
        
        // Redirect to login after a short delay
        setTimeout(() => {
            window.location.href = '/Identity/Account/Login?returnUrl=' + encodeURIComponent(window.location.pathname);
        }, 2000);
    }

    /**
     * GET request
     * @param {string} endpoint - API endpoint
     * @param {object} params - Query parameters
     * @returns {Promise<object>} Response data
     */
    async get(endpoint, params = {}) {
        const queryString = new URLSearchParams(params).toString();
        const url = queryString ? `${endpoint}?${queryString}` : endpoint;
        
        return this.request(url, {
            method: 'GET'
        });
    }

    /**
     * POST request
     * @param {string} endpoint - API endpoint
     * @param {object} data - Request body data
     * @returns {Promise<object>} Response data
     */
    async post(endpoint, data = {}) {
        return this.request(endpoint, {
            method: 'POST',
            body: JSON.stringify(data)
        });
    }

    /**
     * PUT request
     * @param {string} endpoint - API endpoint
     * @param {object} data - Request body data
     * @returns {Promise<object>} Response data
     */
    async put(endpoint, data = {}) {
        return this.request(endpoint, {
            method: 'PUT',
            body: JSON.stringify(data)
        });
    }

    /**
     * DELETE request
     * @param {string} endpoint - API endpoint
     * @returns {Promise<object>} Response data
     */
    async delete(endpoint) {
        return this.request(endpoint, {
            method: 'DELETE'
        });
    }

    /**
     * POST request with FormData (for file uploads)
     * @param {string} endpoint - API endpoint
     * @param {FormData} formData - Form data
     * @returns {Promise<object>} Response data
     */
    async postFormData(endpoint, formData) {
        const authHeader = this.tokenManager.getAuthHeader();
        const headers = {};
        
        if (authHeader) {
            headers['Authorization'] = authHeader;
        }

        try {
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                method: 'POST',
                headers: headers,
                body: formData
            });

            if (response.status === 401) {
                this.handleUnauthorized();
                throw new Error('Unauthorized - please login again');
            }

            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.error || `API Error: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API FormData Request Error:', error);
            throw error;
        }
    }

    // ========== Authentication API ==========

    /**
     * Login with username and password
     * @param {string} username - Username
     * @param {string} password - Password
     * @returns {Promise<object>} Login response with token
     */
    async login(username, password) {
        try {
            const response = await this.post('/auth/login', { username, password });
            
            // Store token
            if (response.token) {
                this.tokenManager.setToken(
                    response.token,
                    response.expiration,
                    {
                        userId: response.userId,
                        username: response.username
                    }
                );
                this.tokenManager.enableApiMode();
            }
            
            return response;
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    }

    /**
     * Logout
     */
    logout() {
        this.tokenManager.clearToken();
        this.tokenManager.disableApiMode();
        window.location.href = '/Identity/Account/Logout';
    }

    /**
     * Register new user
     * @param {object} userData - User registration data
     * @returns {Promise<object>} Registration response
     */
    async register(userData) {
        return this.post('/auth/register', userData);
    }

    // ========== Contacts API ==========

    /**
     * Get company information by search string
     * @param {string} searchStr - Search string (company name or UIC)
     * @returns {Promise<object>} Company information
     */
    async getCompany(searchStr) {
        return this.get('/contacts/company', { searchStr });
    }

    /**
     * Get all contacts
     * @returns {Promise<Array>} List of contacts
     */
    async getContacts() {
        return this.get('/contacts');
    }

    /**
     * Get contacts for DataTables
     * @param {object} dtParams - DataTables parameters
     * @returns {Promise<object} DataTables response
     */
    async getContactsDataTable(dtParams) {
        return this.post('/contacts/datatable', dtParams);
    }

    // ========== Orders API ==========

    /**
     * Get contacts by company ID
     * @param {string} companyId - Company identifier
     * @returns {Promise<Array>} List of contacts
     */
    async getOrderContacts(companyId) {
        return this.get(`/contacts/company/${companyId}`);
    }

    /**
     * Get drivers by company ID
     * @param {string} companyId - Company identifier
     * @returns {Promise<Array>} List of drivers
     */
    async getOrderDrivers(companyId) {
        return this.get(`/orders/drivers/${companyId}`);
    }

    /**
     * Get vehicles by company ID
     * @param {string} companyId - Company identifier
     * @returns {Promise<Array>} List of vehicles
     */
    async getOrderVehicles(companyId) {
        return this.get(`/orders/vehicles/${companyId}`);
    }

    /**
     * Get trailers by company ID
     * @param {string} companyId - Company identifier
     * @returns {Promise<Array>} List of trailers
     */
    async getOrderTrailers(companyId) {
        return this.get(`/orders/trailers/${companyId}`);
    }

    /**
     * Get carriers by area
     * @param {string} area - Area identifier
     * @returns {Promise<Array>} List of carriers
     */
    async getCarriersByArea(area) {
        return this.get(`/orders/carriers/area/${area}`);
    }
}

// Create global instance
window.apiClient = new ApiClient();
