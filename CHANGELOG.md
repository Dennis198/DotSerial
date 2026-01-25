# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/).

---
## [2.0.0] - 2026-01-25

### Added
- Updated Json, Yaml, XMl writting

### Info
- Older versions can't be loaded

---

## [1.2.1] - 2025-11-23

### Added
- Support for objects without public parameterless constructor [Issue](https://github.com/Dennis198/DotSerial/issues/8)

### Fixed
- Resolved a bug for strings with the value "" or string.empty
- Resolved a bug for strings that represent paths. [Issue](https://github.com/Dennis198/DotSerial/issues/9)

---
## [1.2.0] - 2025-10-25

### Added
- Support for YAML serialization and deserialization.

---

## [1.1.0] - 2025-10-17

### Added
- Support for JSON serialization and deserialization.

### Fixed
- Resolved a bug with parsable objects like `DateTime, TimeSpan, ...` type handling during serialization.

---

## [1.0.0] - 2025-09-25

### Added
- Initial release with support for Xml serialization and deserialization.

---