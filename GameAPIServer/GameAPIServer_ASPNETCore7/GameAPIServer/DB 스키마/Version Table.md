# [ Version Table ]

## Master Data Version

```sql

DROP DATABASE IF EXISTS version_db;
CREATE DATABASE IF NOT EXISTS version_db;

USE version_db;

DROP TABLE IF EXISTS version_db.`masterdata_ver`;
CREATE TABLE IF NOT EXISTS version_db.`masterdata_ver`
(
    masterdata_ver INT NOT NULL PRIMARY KEY COMMENT '마스터 데이터 버전',

    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '마스터데이터 버전 테이블';

INSERT INTO version_db.`masterdata_ver` (masterdata_ver) VALUES (10000);
INSERT INTO version_db.`masterdata_ver` (masterdata_ver) VALUES (11000);

```

```
// 인덱스 추가 : 'masterdata_ver' 컬럼은 기본 키로 저정 되어 있지만,
// 테이블에 인덱스를 추가하여 검색 속도를 더 향상 시킬 수 있다.
// 여러 행을 INSERT 하거나 버전 조회 등 다른 쿼리가 있다면 인덱스가 유용하다.
CREATE INDEX idx_masterdata_ver ON version_db.`masterdata_ver` (masterdata_ver);
```

## Game Version
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