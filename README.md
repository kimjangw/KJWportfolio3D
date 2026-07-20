# 기술구현 3D Portfolio

목표 : TimeLine, Cinemachine, Shader Graph를 주로 사용하여 기술 숙달.

| TutorialScene | GameScene |
|-----------|------|
| <img width="600" height="400" alt="image" src="https://github.com/user-attachments/assets/3893a49f-9ec4-4730-aff5-851d781e8368" /> | <img width="400" height="400" alt="image" src="https://github.com/user-attachments/assets/17a2fb5f-69e3-4c1f-b844-a99198b84825" />  |



---

프로젝트 소개
--

| 분류 | 내용 |
|------|------|
| **장르** | 3D 플랫포머 |
| **플랫폼** | PC |
| **엔진** | Unity 6.0 LTS |
| **언어** | C# |
| **IDE** | Visual Studio 2022 Community |
| **버전 관리** | GitHub |
| **개발 인원** | 1인 개발 |
| **개발 기간** | 약 3주 |

---

게임 소개
--
3D 플랫포머를 기반으로  TimeLine + Cinemachine을 같이 사용하여 컷씬을 제작하였습니다.  
스크립트를 이용하여 버튼클릭 시 Inspector값과 Shader Graph를 제어해 플랫폼을 전환합니다.

---

인게임 화면
--

- 플랫폼 제어 (Shader Graph + Script)

| Inspector 제어 | A 상태 | A <-> B 전환 | B 상태 |
|--------|--------|--------|--------|
|  <img width="220" height="160" alt="image" src="https://github.com/user-attachments/assets/9f9bea73-42a9-4dd7-b1cf-d654e9ba826b" /> | <img width="220" height="160"  alt="image" src="https://github.com/user-attachments/assets/cdf81e6b-334e-4c6c-84a1-5c9c2e04941f" /> | <img width="220" height="160"  alt="image" src="https://github.com/user-attachments/assets/1628b567-7b82-4f9b-9b67-9d128d2f4519" /> | <img width="220" height="160"  alt="image" src="https://github.com/user-attachments/assets/7f191d23-7a64-47d7-879c-08d0a8a60f37" />  |  

- 컷씬 (TimeLine + Cinemachine)
  
| 컷씬 1| 컷씬 2 |
|--------|--------|
|<img width="490" height="425" alt="image" src="https://github.com/user-attachments/assets/5721ab9b-85d8-4aff-b94a-d605b36fce45" /> | <img width="490" height="425" alt="image" src="https://github.com/user-attachments/assets/8e42f338-0a08-45aa-8de6-de697d1d103d" />  |

- Cinemachine (ClearShot)

| ClearShot 좌측카메라 | ClearShot 우측카메라 |
|--------|--------|
| <img width="490" height="280" alt="image" src="https://github.com/user-attachments/assets/8c71b944-e3b4-4559-be3f-63e9bb4007c8" /> | <img width="490" height="280" alt="image" src="https://github.com/user-attachments/assets/54c37a26-0357-4243-a05e-3ed56d5a480d" />  |


---

조작법
--

| 키 / 입력 | 동작 |
|-----------|------|
| `W` `A` `S` `D` `Space Bar`| 캐릭터 이동 |
| 마우스 클릭 | 플랫폼 전환 |
| 마우스 이동 | 시점 회전 |

---
핵심 기술
--
- TimeLine
- Cinemachine
- Input System과 BlendTree 연동
- Shader Graph

---
트러블 슈팅
--
- Cinemachine기능 중 ClearShot 구현  


| 예상 범위 밖 클리어샷 시작 | 트리거를 이용한 시작범위 조정 | 적용완료 |
|-----------|------|------|
| <img width="494" height="600" alt="image" src="https://github.com/user-attachments/assets/0935d434-5983-4ac7-b909-eacd5309761d" /> | <img width="458" height="293" alt="image" src="https://github.com/user-attachments/assets/6383556e-e70e-42c3-8d9d-a4cf505b484e" /> | <img width="494" height="600" alt="image" src="https://github.com/user-attachments/assets/72388f25-962c-448d-9dd3-5d2077388c31" /> |

Q : Sinemachine의 ClearShot의 카메라 범위에 들어오자 마자 ClearShot이 발동되어 원하는 거리에서 Sinemachine카메라로 찍으면 구상하던 장면 연출 불가.  
A : 해당 터널에 Box Collider를 설치하고 OnTriggerEnter와 OnTriggerExit 스크립트를 작성하여 시네머신 카메라가 작동되고 꺼지도록 한정하여 문제를 해결.  
