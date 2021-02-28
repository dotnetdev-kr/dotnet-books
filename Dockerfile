FROM python:3.9.0-alpine AS build

WORKDIR /var/build
ADD . .

RUN apk add --no-cache --virtual .build-deps gcc musl-dev git
RUN pip3 install mkdocs \
        mkdocs-material \
        git+https://github.com/jldiaz/mkdocs-plugin-tags

RUN mkdocs build

FROM nginx:stable-alpine

COPY --from=build /var/build/site /usr/share/nginx/html

EXPOSE 80/tcp
