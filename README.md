# Documentation Template

이 리포지터리에는 mkdocs 기반 문서 템플릿을 빠르게 제작할 수 있는 기본 설정이 포함되어있습니다.

## 개발 환경 구축

다음의 소프트웨어가 필요합니다.

- Python 3.8 이상
- 최신 버전의 `pip`
- 최신 버전의 Chromium 기반 브라우저 (Microsoft Edge, Google Chrome 등)

개발 환경 구축을 위하여 다음의 명령어를 실행하여 관련 패키지들을 설치합니다.

```bash
pip install -r ./requirements.txt
```

최적화된 편집을 위하여 WYSIWYG 기반의 마크다운 편집기 또는 Visual Studio Code의 사용을 권장합니다.

## 개발 목적으로 사이트를 실행하는 방법

리포지터리 루트 디렉터리에서 다음과 같이 명령어를 실행합니다.

```bash
mkdocs serve
```

이 상태에서 페이지의 콘텐츠를 고치면 자동으로 최신 내용이 반영되므로 편리하게 작업을 진행할 수 있습니다.

## 정적 페이지 빌드하고 확인하기

리포지터리 루트 디렉터리에서 다음과 같이 명령어를 실행합니다.

```bash
mkdocs build
```

그 다음 아래와 같이 명령어를 입력하여 간이 로컬 서버를 실행합니다.

```bash
cd site
python -m http.server 8000
```

8000번 포트를 사용하기로 하였으므로 브라우저에서 http://localhost:8000 을 열어 접속이 잘 되는지 확인합니다.

## Docker 이미지 빌드하고 확인하기

리포지터리 루트 디렉터리에서 다음과 같이 명령어를 실행합니다.

```bash
docker build -t docs:latest .
```

빌드에 시간이 다소 소요될 수 있습니다. 빌드가 완료된 후에는 다음과 같이 컨테이너를 실행합니다.

```bash
docker run --rm -d -p 8000:80 --name docs docs:latest
```

8000번 포트를 사용하기로 하였으므로 브라우저에서 http://localhost:8000 을 열어 접속이 잘 되는지 확인합니다.

확인을 완료한 후에는 컨테이너를 제거합니다.

```bash
docker rm -f docs
```

## 사이트 배포하기

사이트를 빌드하고 배포하기 위해서는 다음과 같이 특정 브랜치 앞으로 빌드한 static page를 자동으로 commit하고 push하도록 명령어를 실행합니다.

```bash
mkdocs gh-deploy -b published
```

그 후에는 미리 띄워져있는 서버에서 `published` 브랜치의 콘텐츠를 자동으로 Pull 하여 사이트 내용을 새로 고치게 됩니다.
