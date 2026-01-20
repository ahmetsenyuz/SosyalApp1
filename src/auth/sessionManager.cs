using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SosyalApp1.src.auth
{
    public static class SessionManager
    {
        // In-memory storage for demonstration purposes
        // In production, this would be replaced with a proper session store (Redis, database, etc.)
        private static readonly Dictionary<string, SessionInfo> _sessions = new Dictionary<string, SessionInfo>();
        
        // Session timeout in minutes
        private const int SessionTimeoutMinutes = 30;

        /// <summary>
        /// Creates a new session for a user
        /// </summary>
        /// <param name="userId">Unique identifier for the user</param>
        /// <returns>Session token</returns>
        public static string CreateSession(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            // Generate a unique session token
            string sessionToken = GenerateSessionToken();
            
            // Store session info
            _sessions[sessionToken] = new SessionInfo
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                LastAccessed = DateTime.UtcNow
            };

            return sessionToken;
        }

        /// <summary>
        /// Validates if a session token is still valid
        /// </summary>
        /// <param name="sessionToken">Session token to validate</param>
        /// <returns>True if session is valid, false otherwise</returns>
        public static bool ValidateSession(string sessionToken)
        {
            if (string.IsNullOrEmpty(sessionToken))
                return false;

            if (!_sessions.ContainsKey(sessionToken))
                return false;

            SessionInfo session = _sessions[sessionToken];
            
            // Check if session has expired
            if (DateTime.UtcNow > session.CreatedAt.AddMinutes(SessionTimeoutMinutes))
            {
                // Session expired, remove it
                _sessions.Remove(sessionToken);
                return false;
            }

            // Update last accessed time
            session.LastAccessed = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// Invalidates a session (logs out the user)
        /// </summary>
        /// <param name="sessionToken">Session token to invalidate</param>
        /// <returns>True if session was invalidated, false if it didn't exist</returns>
        public static bool InvalidateSession(string sessionToken)
        {
            if (string.IsNullOrEmpty(sessionToken))
                return false;

            return _sessions.Remove(sessionToken);
        }

        /// <summary>
        /// Gets the user ID associated with a session token
        /// </summary>
        /// <param name="sessionToken">Session token</param>
        /// <returns>User ID if session is valid, null otherwise</returns>
        public static string GetUserIdFromSession(string sessionToken)
        {
            if (string.IsNullOrEmpty(sessionToken))
                return null;

            if (!_sessions.ContainsKey(sessionToken))
                return null;

            SessionInfo session = _sessions[sessionToken];
            
            // Check if session has expired
            if (DateTime.UtcNow > session.CreatedAt.AddMinutes(SessionTimeoutMinutes))
            {
                // Session expired, remove it
                _sessions.Remove(sessionToken);
                return null;
            }

            // Update last accessed time
            session.LastAccessed = DateTime.UtcNow;
            return session.UserId;
        }

        /// <summary>
        /// Generates a cryptographically secure random session token
        /// </summary>
        /// <returns>Random session token</returns>
        private static string GenerateSessionToken()
        {
            byte[] randomBytes = new byte[32]; // 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }

    /// <summary>
    /// Represents session information
    /// </summary>
    public class SessionInfo
    {
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}