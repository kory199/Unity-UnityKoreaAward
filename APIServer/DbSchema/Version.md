# [ Version DB ]
  
## User Version Table

```sql
DROP DATABASE IF EXISTS version_db;
CREATE DATABASE IF NOT EXISTS version_db;

USE version_db;

DROP TABLE IF EXISTS version_db.`game_ver`;
CREATE TABLE IF NOT EXISTS version_db.`game_ver`
(
    game_ver INT NOT NULL PRIMARY KEY COMMENT '게임 버전',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '게임 버전 테이블';

INSERT INTO version_db.`game_ver` (game_ver) VALUES (10000);
INSERT INTO version_db.`game_ver` (game_ver) VALUES (11000);
```