# [ Game DB ]
  
## User Game Data Table

```sql

DROP DATABASE IF EXISTS gameDb;
CREATE DATABASE IF NOT EXISTS gameDb;

USE gameDb;

DROP TABLE IF EXISTS gameDb.`player_data`;
CREATE TABLE IF NOT EXISTS gameDb.`player_data`
(
    player_id BIGINT NOT NULL PRIMARY KEY COMMENT '고유 번호', 
    exp INT NOT NULL COMMENT  '경험치',
    hp INT NOT NULL COMMENT '현재 체력',
    score INT NOT NULL COMMENT '점수',
    level INT NOT NULL COMMENT  '레벨',
    status INT DEFAULT 0 NOT NULL COMMENT '상태',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '유저 게임 정보 테이블';
```