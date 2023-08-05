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

## Monster Table

```sql
USE master_dataDb;

DROP TABLE IF EXISTS master_dataDb.`monster_data`;
CREATE TABLE IF NOT EXISTS master_dataDb.`monster_data`
(
    id INT NOT NULL PRIMARY KEY COMMENT '몬스터 고유 아이디',
    type VARCHAR(50) NOT NULL COMMENT '몬스터 타입',
    level INT NOT NULL COMMENT '레벨',
    exp INT NOT NULL COMMENT '경험치',
    hp FLOAT NOT NULL COMMENT '체력',
    speed FLOAT NOT NULL COMMENT '속도',
    rate_of_fire FLOAT NOT NULL COMMENT '발사속도',
    projectile_speed FLOAT NOT NULL COMMENT '발사체 속도',
    collision_damage FLOAT NOT NULL COMMENT '충돌손상',
    score INT NOT NULL COMMENT '점수',
    ranged FLOAT NOT NULL COMMENT '원거리'
) COMMENT '몬스터 데이터 테이블';

INSERT INTO `monster_data` (id, type, level, exp, hp, speed, rate_of_fire, projectile_speed, collision_damage, score, ranged) VALUES
(1, 'MeleeMonstser', 1, 10, 15, 0.5, 0.3, 5, 15, 10, 1),
(2, 'MeleeMonstser', 2, 15, 22.5, 0.525, 0.35, 7.5, 16.5, 20, 1),
(3, 'MeleeMonstser', 3, 20, 30, 0.55, 0.4, 10, 18, 30, 1),
(4, 'MeleeMonstser', 4, 25, 37.5, 0.575, 0.45, 12.5, 19.5, 40, 1),
(5, 'MeleeMonstser', 5, 30, 45, 0.6, 0.5, 15, 21, 50, 1),
(6, 'RangedMonster', 1, 10, 5, 0.5, 0.45, 5, 10, 10, 10),
(7, 'RangedMonster', 2, 15, 7.5, 0.525, 0.525, 7.5, 11, 20, 10),
(8, 'RangedMonster', 3, 20, 10, 0.55, 0.6, 10, 12, 30, 10),
(9, 'RangedMonster', 4, 25, 12.5, 0.575, 0.675, 12.5, 13, 40, 10),
(10, 'RangedMonster', 5, 30, 15, 0.6, 0.75, 15, 14, 50, 10),
(11, 'BOSS', 1, 500, 5000, 1, 1, 1, 10, 500, 15);
```


## Stage Spawn Monster Table
```sql
USE master_dataDb;

DROP TABLE IF EXISTS master_dataDb.`stage_spawn_monster`;
CREATE TABLE IF NOT EXISTS master_dataDb.`stage_spawn_monster`
(
    id INT NOT NULL PRIMARY KEY COMMENT '고유 아이디',
    stage INT NOT NULL COMMENT '스테이지',
    meleemonster_spawn INT NOT NULL COMMENT '근접 몬스터 수량',
    rangedmonster_spawn INT NOT NULL COMMENT '원거리 몬스터 수량'
)COMMENT '스테이지별 생성 몬스터 데이터 테이블';

INSERT INTO master_dataDb.stage_spawn_monster (id, stage, meleemonster_spawn, rangedmonster_spawn)
VALUES
(1, 1, 60, 0),
(2, 2, 30, 30),
(3, 3, 40, 20),
(4, 4, 30, 30),
(5, 5, 36, 24);
```

## PlayerStatus Table
```sql
USE master_dataDb;

DROP TABLE IF EXISTS master_dataDb.`player_status`;
CREATE TABLE IF NOT EXISTS master_dataDb.`player_status`
(
    id INT NOT NULL PRIMARY KEY COMMENT '고유 아이디',
    level INT NOT NULL COMMENT '레벨',
    hp FLOAT NOT NULL COMMENT '체력',
    movement_speed FLOAT NOT NULL COMMENT '이동 속도',
    attack_power INT NOT NULL COMMENT '공격력',
    rate_of_fire FLOAT NULL COMMENT '발사속도',
    projectile_speed FLOAT NOT NULL COMMENT '발사체 속도',
    xp_requiredfor_levelup INT NOT NULL COMMENT '레벨업에 필요한 xp'
)COMMENT '플레이어 상태 테이블';

INSERT INTO master_dataDb.player_status (id, level, hp, movement_speed, attack_power, rate_of_fire, projectile_speed, xp_requiredfor_levelup)
VALUES
(1, 1, 100, 1, 10, 1, 10, 100),
(2, 2, 110, 1.05, 11, 1.1, 11, 200),
(3, 3, 120, 1.1, 12, 1.2, 12, 300),
(4, 4, 130, 1.15, 13, 1.3, 13, 400),
(5, 5, 140, 1.2, 14, 1.4, 14, 500);
```