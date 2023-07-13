# [ Account DB ]
  
## account Table

```sql

DROP DATABASE IF EXISTS accountDb;
CREATE DATABASE IF NOT EXISTS accountDb;

USE accountDb;

DROP TABLE IF EXISTS accountDb.`account`;
CREATE TABLE IF NOT EXISTS accountDb.`account`
(
    account_id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '계정번호',
    id VARCHAR(50) NOT NULL UNIQUE COMMENT '아이디',
    salt_value VARCHAR(100) NOT NULL COMMENT  '암호화 값',
    hashed_password VARCHAR(100) NOT NULL COMMENT '해싱된 비밀번호',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '계정 정보 테이블';
```