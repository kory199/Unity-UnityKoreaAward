# [ Account DB ]
  
## account Table

```sql

DROP DATABASE IF EXISTS account_db;
CREATE DATABASE IF NOT EXISTS account_db;

USE account_db;

DROP TABLE IF EXISTS account_db.`account`;
CREATE TABLE IF NOT EXISTS account_db.`account`
(
    account_id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '계정번호',
    id VARCHAR(50) NOT NULL UNIQUE COMMENT '아이디',
    salt_value VARCHAR(100) NOT NULL COMMENT  '암호화 값',
    hashed_password VARCHAR(100) NOT NULL COMMENT '해싱된 비밀번호',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '계정 정보 테이블';
```


## Terms of Use Table

```sql
DROP DATABASE IF EXISTS account_db;
CREATE DATABASE IF NOT EXISTS account_db;

USE account_db;

DROP TABLE IF EXISTS account_db.`termsofuse`;
CREATE TABLE IF NOT EXISTS account_db.`termsofuse`
(
    account_id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '계정번호',
    use_game_service INT NOT NULL COMMENT '게임서비스 이용동의',
    get_user_info INT NOT NULL COMMENT  '개인 정보 동의',
    receive_user_event INT NOT NULL COMMENT '이벤트 수신 동의',
    receive_night_event INT NOT NULL COMMENT '야간 이벤트 수신 동의',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '체크한 날짜'
) COMMENT '게임 이용 약관 동의 테이블';

```

# [ Game DB ]

## GameData Table
월드에 존재하는 유저에 대한 정보

```sql

DROP DATABASE IF EXISTS game_db;
CREATE DATABASE IF NOT EXISTS game_db;

USE game_db;

DROP TABLE IF EXISTS game_db.`player_data`;
CREATE TABLE IF NOT EXISTS game_db.`player_data`
(
    player_id BIGINT NOT NULL PRIMARY KEY COMMENT '고유 번호', 
    exp INT NOT NULL COMMENT  '경험치',
    level INT NOT NULL COMMENT  '레벨',
    hp INT NOT NULL COMMENT '현재 체력',

    mp INT NOT NULL COMMENT '현재 마력',
    gold BIGINT NOT NULL COMMENT '소지한 머니',
    status INT DEFAULT 0 NOT NULL COMMENT '상태 (기본, 삭제)', // ** 삭제
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '유저 게임의 정보';
```
  
## Item Table
보유한 아이템에 대한 정보  
  
```sql

DROP DATABASE IF EXISTS game_db;
CREATE DATABASE IF NOT EXISTS game_db;

USE game_db;

DROP TABLE IF EXISTS game_db.`player_item`;
CREATE TABLE IF NOT EXISTS game_db.`player_item`
(
    player_id BIGINT NOT NULL COMMENT '고유 번호',
    item_code INT NOT NULL PRIMARY KEY COMMENT '아이템 코드',
    count BIGINT NOT NULL COMMENT '보유 수량',
    attack INT NOT NULL COMMENT '공격력',
    defence INT NOT NULL COMMENT '방어력',
    magic INT NOT NULL COMMENT '마법력',
    enhance_count INT NOT NULL COMMENT '강화 횟수',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '보유 아이템의 정보'; 
```


##  MasterData Version Table
```
DROP DATABASE IF EXISTS 67game_db;
CREATE DATABASE IF NOT EXISTS game_db;

USE game_db;
 
DROP TABLE IF EXISTS game_db.`masterdata_version`;
CREATE TABLE IF NOT EXISTS game_db.`masterdata_version`
(
    master_version INT UNSIGNED NOT NULL PRIMARY KEY COMMENT '마스터 데이터 버전'
) COMMENT '마스터데이터 버전의 정보';

INSERT INTO masterdata_version (master_version)
VALUES (1);
```

##  Player Version Table
```
DROP DATABASE IF EXISTS game_db;
CREATE DATABASE IF NOT EXISTS game_db;

USE game_db;
 
DROP TABLE IF EXISTS game_db.`player_version`;
CREATE TABLE IF NOT EXISTS game_db.`player_version`
(
    player_id BIGINT NOT NULL PRIMARY KEY COMMENT '유저 고유 번호',
    version INT UNSIGNED NOT NULL UNIQUE COMMENT '유저 현재 버전'
) COMMENT '버전의 정보';

```

##  Dungeon Stage Table

USE game_db;

DROP TABLE IF EXISTS game_db.`dungeon_stage`;
CREATE TABLE IF NOT EXISTS game_db.`dungeon_stage`
(
    player_id BIGINT NOT NULL COMMENT '유저 고유 번호',
    stage_id BIGINT NOT NULL PRIMARY KEY COMMENT '완료 스테이지',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '던전 스테이지의 정보';

INSERT INTO game_db.`dungeon_stage` (player_id, stage_id)
VALUES(1, 1);


## Attendance Table
```
USE game_db;

DROP TABLE IF EXISTS game_db.`attendance`;
CREATE TABLE IF NOT EXISTS game_db.`attendance`
(
    player_id BIGINT NOT NULL PRIMARY KEY COMMENT '유저 고유 번호',
    attendance_day DATETIME NOT NULL COMMENT '출석 날짜',
    continuous_attendance INT NOT NULL UNIQUE COMMENT '연속 출석 일수'
) COMMENT '유저 출석의 정보';
```


## InAppPay Table

```
USE game_db;

DROP TABLE IF EXISTS game_db.`inapp`;
CREATE TABLE IF NOT EXISTS game_db.`inapp`
(
    id INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '고유 번호',
    player_id BIGINT NOT NULL COMMENT '유저 고유 번호',
    receipt_number BIGINT NOT NULL COMMENT '영수증 번호',
    item_code INT NOT NULL COMMENT '아이템 코드',
    count INT NOT NULL COMMENT '지급 개수',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜'
) COMMENT '유저 인앱 결제의 정보';
```