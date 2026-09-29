<div align="center">
  <h1 align="center">KitsuneLab©</h1>
  <h3 align="center">K4 - Arenas</h3>
  <a align="center">An all-in-one arena plugin for Counter-Strike 2 with ladder-style gameplay. Supports any map, 2v2/3v3 modes, weapon preferences and a developer API for custom round types.</a>
</div>

<p align="center">
  <img src="https://img.shields.io/badge/build-passing-brightgreen" alt="Build Status">
  <img src="https://img.shields.io/github/downloads/Shmitzas/K4-Arenas-Upkeep/total?style=flat&logo=github&cacheSeconds=3600" alt="Downloads">
  <img src="https://img.shields.io/github/stars/Shmitzas/K4-Arenas-Upkeep?style=flat&logo=github&cacheSeconds=3600" alt="Stars">
  <img src="https://img.shields.io/github/license/Shmitzas/K4-Arenas-Upkeep" alt="License">
</p>

# Important notice!
> [!IMPORTANT]  
> [K4ryuu](https://github.com/K4ryuu) is the creator of this plugin.<br>
> Since he is no longer maintaining his CS2 plugins, I forked some of them and maintain them ONLY FOR BUG FIXES!

---

### Dependencies

- [**SwiftlyS2**](https://github.com/swiftly-solution/swiftlys2): Server plugin framework for Counter-Strike 2
- **Database**: One of the following supported databases:
  - **MySQL / MariaDB** - Recommended for production
  - **PostgreSQL** - Full support
  - **SQLite** - Great for single-server setups

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- INSTALLATION -->

## Installation

1. Install [SwiftlyS2](https://github.com/swiftly-solution/swiftlys2) on your server
2. Configure your database connection in SwiftlyS2's `database.jsonc` (MySQL, PostgreSQL, or SQLite)
3. [Download the latest release](https://github.com/shmitzas/K4-Arenas-Upkeep/releases/latest)
4. Extract to your server's `swiftlys2/plugins/` directory
5. Configure `config.json` in the plugin folder
6. Restart your server - database tables will be created automatically

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONFIGURATION -->

## Configuration

| Option               | Description                                      | Default    |
| -------------------- | ------------------------------------------------ | ---------- |
| `DatabaseConnection` | Database connection name for storing preferences | `""`       |
| `MinPlayersToStart`  | Minimum players required to start arenas         | `2`        |
| `RoundTimeSeconds`   | Round duration in seconds                        | `60`       |
| `ArenaAfkCommand`    | Command to toggle AFK status                     | `"afk"`    |
| `ArenaGunsCommand`   | Command to open weapon preferences               | `"guns"`   |
| `ArenaRoundsCommand` | Command to open round preferences                | `"rounds"` |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- COMMANDS -->

## Commands

| Command   | Description                      |
| --------- | -------------------------------- |
| `!guns`   | Open weapon preferences menu     |
| `!rounds` | Open round type preferences menu |
| `!afk`    | Toggle AFK status                |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Database

The plugin uses automatic schema management with FluentMigrator. Tables are created automatically on first run.

### Supported Databases

| Database        | Status  | Notes                                      |
| --------------- | ------- | ------------------------------------------ |
| MySQL / MariaDB | ✅ Full | Recommended for multi-server setups        |
| PostgreSQL      | ✅ Full | Alternative for existing Postgres setups   |
| SQLite          | ✅ Full | Perfect for single-server, no setup needed |

### Database Tables

- `k4_arenas_players` - Player records and last seen timestamps
- `k4_arenas_weapons` - Player weapon preferences per weapon type
- `k4_arenas_rounds` - Player round type preferences

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->

## License

Distributed under the GPL-3.0 License. See [`LICENSE.md`](LICENSE.md) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
