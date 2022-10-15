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

## Version API

When build the new image, please increase the `APIVersion` in file `appsettings.json`;
API check version: `/api/Common/Version`

## Jenkins

- `JenkinsfileBuild` used to config pipeline to: clone repo --> build image --> pubish image to docker hub
- `JenkinsfileRun` used to config pipeline to: ssh remote server --> pull image from dockerhub --> run container

## TODO

[ ] Validate field model