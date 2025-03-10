# Real-Time Leaderboard System

## Overview

This project is a **real-time leaderboard system** that ranks users based on their scores in various games or activities. The system leverages **Redis Sorted Sets** to efficiently store and manage leaderboards while providing real-time updates.

## Features

- **User Authentication**: Register and log in using JWT-based authentication.
- **Score Submission**: Users can submit scores for different games or activities.
- **Global Leaderboard**: Displays the top users across all games.
- **User Rankings**: Users can view their current ranking on the leaderboard.
- **Players Scores Report**: Generates reports on the scores of players for a specific period of time.
- **Real-Time Updates**: Utilizes Redis to ensure fast leaderboard updates and queries.
- **Rank Queries**: Uses Redis commands to fetch user rankings efficiently.

## Tech Stack

- **Backend**: ASP.NET Core Minimal APIs
- **Authentication**: JWT (JSON Web Token)
- **Database**: PostgreSQL and Entity FrameWork
- **Caching**: Redis Sorted Sets (for real-time ranking updates)

## Setup Instructions

### Prerequisites

- .NET 8 SDK or later
- PostgreSQL server installed and running
- Redis server installed and running


### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/moyaser315/RealTimeLeaderBoard.git
   cd RealTimeLeaderBoard
   ```

2. Install dependencies:

   ```bash
   dotnet restore
   ```

3. Configure PostgreSQL & Redis connection in `appsettings.json`:

   ```json
   {
    "ConnectionStrings": {
        "AppDb": "Host=localhost;Database=leaderboard;Username=postgres;Password=admin",
        "Redis": "localhost:6379"
    }
   }
   ```

4. Run the application:

   ```bash
   dotnet run
   ```

## API Endpoints

It can be found in ```LeadrBoard.http``` file.


## Future Enhancements

- [ ] Implement WebSockets for further real-time enhancmnets.
- [ ] Add frontend dashboard for visualization.
- [ ] Add docker files.

