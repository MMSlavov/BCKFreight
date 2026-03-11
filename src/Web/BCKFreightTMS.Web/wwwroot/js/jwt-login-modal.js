/**
 * JWT Login Modal
 * Provides a modal dialog for API authentication
 */

// Create and inject login modal HTML
const jwtLoginModalHtml = `
<div class="modal fade" id="jwtLoginModal" tabindex="-1" role="dialog" aria-labelledby="jwtLoginModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title" id="jwtLoginModalLabel">
                    <i class="fas fa-key"></i> API Login
                </h5>
                <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <form id="jwtLoginForm">
                    <div class="alert alert-info">
                        <i class="fas fa-info-circle"></i> 
                        Login with your credentials to access the application via JWT authentication.
                    </div>
                    
                    <div class="form-group">
                        <label for="jwtUsername">Username</label>
                        <input type="text" class="form-control" id="jwtUsername" name="username" required 
                               placeholder="Enter your username">
                    </div>
                    
                    <div class="form-group">
                        <label for="jwtPassword">Password</label>
                        <input type="password" class="form-control" id="jwtPassword" name="password" required 
                               placeholder="Enter your password">
                    </div>
                    
                    <div id="jwtLoginError" class="alert alert-danger" style="display: none;">
                        <i class="fas fa-exclamation-triangle"></i> 
                        <span id="jwtLoginErrorText"></span>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                    <i class="fas fa-times"></i> Cancel
                </button>
                <button type="button" class="btn btn-primary" id="jwtLoginBtn">
                    <i class="fas fa-sign-in-alt"></i> Login with JWT
                </button>
            </div>
        </div>
    </div>
</div>
`;

// Inject modal into page
$(document).ready(function() {
    $('body').append(jwtLoginModalHtml);
    
    // Initialize JWT status
    updateJwtStatus();
    
    // Update JWT status every 30 seconds
    setInterval(updateJwtStatus, 30000);
    
    // Handle login form submission
    $('#jwtLoginBtn').on('click', handleJwtLogin);
    $('#jwtLoginForm').on('submit', function(e) {
        e.preventDefault();
        handleJwtLogin();
    });
    
    // Handle JWT logout
    $('#jwtLogoutBtn').on('click', handleJwtLogout);
    
    // Add API login button to navigation (if not exists)
    addApiLoginButton();
});

/**
 * Handle JWT login
 */
async function handleJwtLogin() {
    const username = $('#jwtUsername').val();
    const password = $('#jwtPassword').val();
    
    if (!username || !password) {
        showJwtLoginError('Please enter both username and password');
        return;
    }
    
    // Disable button and show loading
    const $btn = $('#jwtLoginBtn');
    const originalHtml = $btn.html();
    $btn.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Logging in...');
    
    // Hide any previous errors
    $('#jwtLoginError').hide();
    
    try {
        // Call API login
        const response = await window.apiClient.login(username, password);
        
        // Success
        console.log('JWT login successful', response);
        
        // Show success notification
        if (typeof notyf !== 'undefined') {
            notyf.success('Successfully logged in with JWT!');
        }
        
        // Close modal
        $('#jwtLoginModal').modal('hide');
        
        // Clear form
        $('#jwtLoginForm')[0].reset();
        
        // Update status
        updateJwtStatus();
        
        // Reload page to apply API mode
        setTimeout(() => {
            window.location.reload();
        }, 1000);
        
    } catch (error) {
        console.error('JWT login error:', error);
        showJwtLoginError(error.message || 'Login failed. Please check your credentials.');
        
    } finally {
        // Re-enable button
        $btn.prop('disabled', false).html(originalHtml);
    }
}

/**
 * Handle JWT logout
 */
function handleJwtLogout() {
    if (confirm('Are you sure you want to logout?')) {
        console.log('Logging out and clearing JWT token...');
        
        // Clear JWT token
        window.jwtManager.clearToken();
        
        if (typeof notyf !== 'undefined') {
            notyf.info('Logged out successfully.');
        }
        
        updateJwtStatus();
        
        // Redirect to logout page (which will also sign out of cookie auth)
        setTimeout(() => {
            window.location.href = '/Identity/Account/Logout';
        }, 500);
    }
}

/**
 * Show login error
 */
function showJwtLoginError(message) {
    $('#jwtLoginErrorText').text(message);
    $('#jwtLoginError').fadeIn();
}

/**
 * Update JWT status indicator
 */
function updateJwtStatus() {
    const $indicator = $('#jwtStatusIndicator');
    const $timeRemaining = $('#jwtTimeRemaining');
    
    if (window.jwtManager && window.jwtManager.isAuthenticated()) {
        const seconds = window.jwtManager.getTimeUntilExpiry();
        const minutes = Math.floor(seconds / 60);
        
        if (minutes > 0) {
            $timeRemaining.text(`(${minutes}m)`);
            $indicator.fadeIn();
        } else {
            $indicator.fadeOut();
        }
    } else {
        $indicator.fadeOut();
    }
}

/**
 * Add API login button to navigation
 */
function addApiLoginButton() {
    const $userMenu = $('.navbar-nav .nav-item.dropdown').first();
    
    if ($userMenu.length && !$('#apiLoginMenuItem').length) {
        const isAuthenticated = window.jwtManager && window.jwtManager.isAuthenticated();
        
        const menuItem = `
            <a class="dropdown-item" href="#" id="apiLoginMenuItem" data-toggle="modal" data-target="#jwtLoginModal">
                <i class="fas fa-key"></i> 
                ${isAuthenticated ? 'JWT Authenticated' : 'Login'}
            </a>
        `;
        
        $userMenu.find('.dropdown-menu').append('<div class="dropdown-divider"></div>');
        $userMenu.find('.dropdown-menu').append(menuItem);
    }
}

/**
 * Global function to show JWT login modal
 */
window.showJwtLoginModal = function() {
    $('#jwtLoginModal').modal('show');
};
