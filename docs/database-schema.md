# Database Schema Design

## Entities and Relationships

This document outlines the core data models and database schema for the social task and score tracking application.

### User Entity
- **Fields:**
  - username (string, unique)
  - profilePicture (string, URL)
  - totalPoints (integer)
  - completedTasksCount (integer)

### Task Entity
- **Fields:**
  - description (string)
  - difficultyLevel (string: easy, medium, hard)
  - pointValue (integer)
  - status (string: pending, in-progress, completed)

### Friendship Entity
- **Fields:**
  - userId (string, foreign key to User)
  - friendId (string, foreign key to User)
  - status (string: pending, accepted, rejected)

### Score Entity
- **Fields:**
  - userId (string, foreign key to User)
  - taskId (string, foreign key to Task)
  - pointsEarned (integer)
  - timestamp (datetime)
  - aggregationType (string: daily, weekly, monthly)

### Evidence Photo Entity
- **Fields:**
  - taskId (string, foreign key to Task)
  - userId (string, foreign key to User)
  - imageUrl (string, URL)
  - uploadDate (datetime)
  - description (string)

## Relationships
- One User can have many Tasks
- One User can have many Scores
- One User can have many Evidence Photos
- One Task can have many Scores
- One Task can have many Evidence Photos
- Users can have many Friendships (one-to-many)