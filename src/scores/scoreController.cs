using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace SosyalApp1.src.scores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        // Get total points for a user
        [HttpGet("user/{userId}/total")]
        public IActionResult GetTotalPoints(int userId)
        {
            try
            {
                var totalPoints = ScoreService.GetTotalPointsForUser(userId);
                return Ok(new { userId = userId, totalPoints = totalPoints });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to calculate total points", details = ex.Message });
            }
        }

        // Get daily score for a user
        [HttpGet("user/{userId}/daily")]
        public IActionResult GetDailyScore(int userId, [FromQuery] DateTime date)
        {
            try
            {
                var dailyScore = ScoreService.GetDailyScoreForUser(userId, date);
                return Ok(new { userId = userId, date = date, dailyScore = dailyScore });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to calculate daily score", details = ex.Message });
            }
        }

        // Get weekly score for a user
        [HttpGet("user/{userId}/weekly")]
        public IActionResult GetWeeklyScore(int userId, [FromQuery] DateTime weekStartDate)
        {
            try
            {
                var weeklyScore = ScoreService.GetWeeklyScoreForUser(userId, weekStartDate);
                return Ok(new { userId = userId, weekStartDate = weekStartDate, weeklyScore = weeklyScore });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to calculate weekly score", details = ex.Message });
            }
        }

        // Get monthly score for a user
        [HttpGet("user/{userId}/monthly")]
        public IActionResult GetMonthlyScore(int userId, [FromQuery] DateTime monthStartDate)
        {
            try
            {
                var monthlyScore = ScoreService.GetMonthlyScoreForUser(userId, monthStartDate);
                return Ok(new { userId = userId, monthStartDate = monthStartDate, monthlyScore = monthlyScore });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to calculate monthly score", details = ex.Message });
            }
        }

        // Get score history for a user
        [HttpGet("user/{userId}/history")]
        public IActionResult GetScoreHistory(int userId)
        {
            try
            {
                var scoreHistory = ScoreService.GetScoreHistoryForUser(userId);
                return Ok(new { userId = userId, scoreHistory = scoreHistory });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to retrieve score history", details = ex.Message });
            }
        }

        // Update point values for difficulty levels
        [HttpPut("point-values")]
        public IActionResult UpdatePointValues([FromBody] Dictionary<string, int> newPointValues)
        {
            try
            {
                ScoreService.UpdatePointValues(newPointValues);
                return Ok(new { message = "Point values updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to update point values", details = ex.Message });
            }
        }
    }
}