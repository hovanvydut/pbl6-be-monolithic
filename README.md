## PBL6 - BE

## Local
```sh
cd /monolithic
make setup
make run
```

## Build container & run
```
make dc-up
```

## Jenkins

- `JenkinsfileBuild` used to config pipeline to: clone repo --> build image --> pubish image to docker hub
- `JenkinsfileRun` used to config pipeline to: ssh remote server --> pull image from dockerhub --> run container