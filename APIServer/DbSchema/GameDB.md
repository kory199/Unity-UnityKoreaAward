# [ Game DB ]
  
## User Game Data Table

```sql
DROP DATABASE IF EXISTS gameDb;
CREATE DATABASE IF NOT EXISTS gameDb;

USE gameDb;

DROP TABLE IF EXISTS gameDb.`player_data`;
CREATE TABLE IF NOT EXISTS gameDb.`player_data`
(
    player_uid BIGINT NOT NULL PRIMARY KEY COMMENT '고유 번호', 
    id VARCHAR(50) NOT NULL UNIQUE COMMENT '아이디',
    exp INT NOT NULL COMMENT  '경험치',
    hp INT NOT NULL COMMENT '현재 체력',
    score INT NOT NULL COMMENT '점수',
    level INT NOT NULL COMMENT  '레벨',
    status INT DEFAULT 0 NOT NULL COMMENT '상태',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '유저 게임 정보 테이블';
```

## Stage Table

```sql

USE gameDb;

DROP TABLE IF EXISTS gameDb.`stage`;
CREATE TABLE IF NOT EXISTS gameDb.`stage`
(
    stage_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '스테이지 ID', 
    prev_stage_id INT DEFAULT NULL COMMENT '이전 스테이지 ID',
    FOREIGN KEY (prev_stage_id) REFERENCES gameDb.`stage`(stage_id)
) COMMENT '스테이지 정보 테이블';
```

``` sql
USE gameDb;

DROP TABLE IF EXISTS gameDb.`player_stage`;
CREATE TABLE IF NOT EXISTS gameDb.`player_stage`
(
    player_uid BIGINT NOT NULL COMMENT '고유 번호', 
    stage_id INT NOT NULL CHECK (stage_id BETWEEN 1 AND 5) COMMENT '스테이지 ID',
    is_achieved BOOLEAN NOT NULL DEFAULT 0 COMMENT '스테이지 달성 여부',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜',
    UNIQUE (player_uid, stage_id)
) COMMENT '유저와 스테이지 정보 연결 테이블';

```
