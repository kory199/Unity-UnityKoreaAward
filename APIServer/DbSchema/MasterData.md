# [ MasterData DB ]
  
## MasterData Table

```sql
DROP DATABASE IF EXISTS master_dataDb;
CREATE DATABASE IF NOT EXISTS master_dataDb;

USE master_dataDb;

DROP TABLE IF EXISTS master_dataDb.`game_ver`;
CREATE TABLE IF NOT EXISTS master_dataDb.`game_ver`
(
    version INT NOT NULL PRIMARY KEY COMMENT '게임 버전',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '게임 버전 테이블';

INSERT INTO master_dataDb.`game_ver` (version) VALUES (10000);
INSERT INTO master_dataDb.`game_ver` (version) VALUES (11000);
```