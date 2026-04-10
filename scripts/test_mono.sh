#!/bin/bash
docker run --rm -v "$(pwd):/app" -w /app ubuntu:20.04 bash -c "
  apt-get update && DEBIAN_FRONTEND=noninteractive apt-get install -y mono-devel mono-xsp4 &&
  aspnet_compiler -v / -p /app -u /app/publish_output
"
