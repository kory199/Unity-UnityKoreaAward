# [ MasterData DB ]

## Item Table

```sql

DROP DATABASE IF EXISTS masterdata_db;
CREATE DATABASE IF NOT EXISTS masterdata_db;

USE masterdata_db;

DROP TABLE IF EXISTS masterdata_db.`item`;
CREATE TABLE IF NOT EXISTS masterdata_db.`item`
(
    code INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '아이템 번호',
    name VARCHAR(50) NOT NULL UNIQUE COMMENT '아이템 이름',
    attribute INT NOT NULL COMMENT '특성',
    sell BIGINT NOT NULL COMMENT '판매 금액',
    buy BIGINT NOT NULL COMMENT '구입 금액',
    use_lv INT NOT NULL COMMENT '사용 가능 레벨',
    attack BIGINT NOT NULL COMMENT '공격력',
    defence BIGINT NOT NULL COMMENT '방어력',
    magic BIGINT NOT NULL COMMENT '마법력',
    enhance_max_count TINYINT NOT NULL COMMENT '최대 강화 가능 횟수'
) COMMENT '아이템 정보 테이블';

INSERT into item (name, attribute, sell, buy, use_lv, attack, defence, magic, enhance_max_count)
VALUES 
('돈', 5, 0, 0, 0, 0, 0, 0, 0),
('작은 칼', 1, 10, 20, 1, 10, 5, 1, 10),
('도금 칼', 1, 100, 200, 5, 29, 12, 10, 10),
('나무 방패', 2, 7, 15, 1, 3, 10, 1, 10),
('보통 모자', 3, 5, 8, 1, 1, 1, 1, 10),
('포션', 4, 3, 6, 1, 0, 0, 0, 0);

```

```sql

DROP DATABASE IF EXISTS masterdata_db;
CREATE DATABASE IF NOT EXISTS masterdata_db;

USE masterdata_db;

DROP TABLE IF EXISTS masterdata_db.`item_attribute`;
CREATE TABLE IF NOT EXISTS masterdata_db.`item_attribute`
(
    code INT AUTO_INCREMENT NOT NULL PRIMARY KEY COMMENT '아이템 값',
    name VARCHAR(50) NOT NULL COMMENT '특성 이름'
) COMMENT '아이템 특성 테이블';

INSERT INTO item_attribute (name) 
VALUES ("무기"), ("방어구"), ("복장"), ("마법도구"), ("돈");
```

```sql

DROP DATABASE IF EXISTS masterdata_db;
CREATE DATABASE IF NOT EXISTS masterdata_db;

USE masterdata_db;

DROP TABLE IF EXISTS masterdata_db.`attendance_reward`;
CREATE TABLE IF NOT EXISTS masterdata_db.`attendance_reward`
(
    code INT AUTO_INCREMENT NOT NULL PRIMARY KEY COMMENT '날짜',
    item_code INT NOT NULL COMMENT '아이템 코드',
    count INT NOT NULL COMMENT '개수'
) COMMENT '출석부 보상 테이블';

INSERT INTO attendance_reward (item_code, count)
VALUES
(1, 100),
(1, 100),
(1, 100),
(1, 200),
(1, 200),
(1, 200),
(2, 1),
(1, 100),
(1, 100),
(1, 100),
(6, 5),
(1, 150),
(1, 150),
(1, 150),
(1, 150),
(1, 150),
(1, 150),
(4, 1),
(1, 200),
(1, 200),
(1, 200),
(1, 200),
(1, 200),
(5, 1),
(1, 250),
(1, 250),
(1, 250),
(1, 250),
(1, 250),
(3, 1);
```

```sql
DROP DATABASE IF EXISTS masterdata_db;
CREATE DATABASE IF NOT EXISTS masterdata_db;

USE masterdata_db;

DROP TABLE IF EXISTS masterdata_db.`in_app_product`;
CREATE TABLE IF NOT EXISTS masterdata_db.`in_app_product`
(
    code INT NOT NULL COMMENT '상품 번호',
    item_code INT NOT NULL COMMENT '아이템 코드',
    item_name VARCHAR(50) NOT NULL COMMENT '아이템 이름',
    item_count BIGINT NOT NULL COMMENT '개수'
) COMMENT '인앱 결제 테이블';

INSERT INTO in_app_product (code, item_code,item_name, item_count)
VALUES
(1, 1, '돈', 1000),
(1, 2, '작은 칼', 1),
(1, 3, '도금 칼', 1),
(2, 4, '나무 방패', 1),
(2, 5, '보통 모자', 1),
(2, 6, '포션', 10),
(3, 1, '돈', 2000),
(3, 2, '작은 칼', 1),
(3, 3, '나무 방패', 1),
(3, 5, '보통 모자', 1);

```

```
DROP DATABASE IF EXISTS masterdata_db;
CREATE DATABASE IF NOT EXISTS masterdata_db;

USE masterdata_db;

DROP TABLE IF EXISTS masterdata_db.`stage_item`;
CREATE TABLE IF NOT EXISTS masterdata_db.`stage_item`
(
    code INT NOT NULL COMMENT '스테이지 단계',
    item_code INT NOT NULL COMMENT '파밍 가능 아이템'
) COMMENT '스테이지 아이템 테이블';

INSERT INTO stage_item (code, item_code)
VALUES
(1, 1),
(1, 2),
(2, 3),
(2, 3);

```

```sql

DROP DATABASE IF EXISTS masterdata_db;
CREATE DATABASE IF NOT EXISTS masterdata_db;

USE masterdata_db;

DROP TABLE IF EXISTS masterdata_db.`stage_attack_npc`;
CREATE TABLE IF NOT EXISTS masterdata_db.`stage_attack_npc`
(
     code INT NOT NULL COMMENT '스테이지 단계',
     npc_code INT NOT NULL PRIMARY KEY COMMENT '공격 NPC',
     count INT NOT NULL COMMENT '개수',
     exp INT NOT NULL UNIQUE COMMENT '1개당 보상 경험치'
) COMMENT '스테이지 공격 NPC 테이블';

INSERT INTO stage_attack_npc (code, npc_code, count, exp)
VALUES
(1, 101, 10, 10),
(1, 110, 12, 15),
(2, 201, 40, 20),
(2, 211, 20, 35),
(2, 221, 1, 50);

```