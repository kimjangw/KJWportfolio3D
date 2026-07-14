# 기술구현 3D Portfolio

목표 : TimeLine, Sinemachine, Shader Graph를 주로 사용하여 기술 숙달.

| TutorialScene | GameScene |
|-----------|------|
| <img width="400" height="400" alt="image" src="https://github.com/user-attachments/assets/3893a49f-9ec4-4730-aff5-851d781e8368" /> | <img width="400" height="400" alt="image" src="https://github.com/user-attachments/assets/17a2fb5f-69e3-4c1f-b844-a99198b84825" /> |



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
3D 플랫포머를 기반으로  TimeLine + Sinemachine을 같이 사용하여 컷씬을 제작하였습니다.  
스크립트를 이용하여 버튼클릭 시 Inspector값과 Shader Graph를 제어하여 플랫폼을 전환합니다.

---

인게임 화면
--

- 컷씬
<img width="1567" height="884" alt="image" src="https://github.com/user-attachments/assets/02922ebd-6a1f-457a-bc4b-21c344c47392" />

- Sinemachine

- 플랫폼 제어
<img width="1353" height="750" alt="image" src="https://github.com/user-attachments/assets/581d0356-e52d-4005-ad14-610d54cceee8" />
<img width="1342" height="748" alt="image" src="https://github.com/user-attachments/assets/002e3281-5ba6-4b9d-a3e4-8bb6bf57c0d1" />

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
- Sinemachine
- Input System과 BlendTree 연동
- Shader Graph

---
트러블 슈팅
--
- URP에 대한 이해


| 단면 렌더링 | Shader Graph 작업 | 양면 렌더링 |
|-----------|------|------|
| <img width="283" height="346" alt="image" src="https://github.com/user-attachments/assets/e5e04d4b-a1c7-4d8d-a373-eea71ab9b136" /> | <img width="390" height="327" alt="image" src="https://github.com/user-attachments/assets/c39fc806-d98a-4325-a6ce-c14522619457" /> | <img width="284" height="335" alt="image" src="https://github.com/user-attachments/assets/5cf6bfa4-c4f8-419d-b3c4-b984f0733ced" /> |

Q : 해당 커튼은 8번출구 게임처럼 꺾인 복도 형태가 아니기 때문에 전시실을 가릴 필요가 있었으나 단면 렌더링에 의해 안쪽이 보이는 이슈 발생  
A : Shader Graph를 이용하여 양면 렌더링을 진행하였고, 후에 URP의 원리를 배운 후 Backface Culling의 최적화에 의해 뒷면이 안보이는 것을 인식하였고 법선 벡터의 각도와 카메라의 각도를 계산하여 0미만이면 뒷면이라고 판정하는 것을 이해하였습니다.
