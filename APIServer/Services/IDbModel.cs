namespace APIServer.Services;

public interface IDbModel
{
    // 데이터베이스 테이블 이름을 가져오는 속성
    public String TableName { get; } 
}