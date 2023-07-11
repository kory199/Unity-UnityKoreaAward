## 7월 10일(월) 개발일지
### 개발 사항 : 
1. 게임 기획 초안 미완성으로, 우선 CreateAccountAPI 및 LoginAPI 기본 구현
2. Ngork 터널링 툴을 이용한 로컬 서버 외부 접근 허용
3. 유니티로 LoginAPI 간단한 테스트 코드 작성
4. 최신 게임을 다운받아 플레이해 본 뒤 구글 및 facebook 계정 연동 구현의 필요성을 인지하여 구글 로그인 연동부터 구현 시작 (미완성)
   
### 오늘 개발 이슈 : 
1. Properties 폴더의 sslPort 설정 및 appsettings.json prot 설정 불일치로 서버 연결 오류
2. 라우팅 및 미들웨어 추가 부주의로 인한 라우팅 오류
3. google login API 호출하면 로그인 페이지 생성 안 됨

### 받은 피드백 : 
1. 스스로 혼자 지정한 ErrorCode만 Response를 보내는 상황, 받는 클라이언트입장을 더 생각 해야한다.
   ResultCode와 메시지를 같이 보내야 한다.
2. Requset의 성공과 실패 여부 추가.

### 7월 11일(화) (예정)
1. 구글 로그인 연동
2. 페이스북 로그인 연동
3. Response 디테일하게 작성
4. 테스터 용이성을 위해 LocalTunnel 이용해 서브 도메인으로 APIURL 생성하기
