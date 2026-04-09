# LearnSite 信息技术学习平台

[![Gitee 最新发行版](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fgitee.com%2Fapi%2Fv5%2Frepos%2Fjnschool%2Flearnsite-wz%2Ftags%3F%26per_page%3D1%26direction%3Ddesc%26sort%3Dupdated&query=%24%5B0%5D.name&logo=gitee&label=%E6%9C%80%E6%96%B0%E7%89%88%E6%9C%AC&color=c71d23)](https://gitee.com/jnschool/learnsite-wz)
[![Docker latest 更新](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fhub.docker.com%2Fv2%2Frepositories%2Forzg%2Flearnsite-web%2Ftags%2Flatest&query=%24.last_updated&logo=docker&label=latest%20更新&color=2496ED)](https://hub.docker.com/r/orzg/learnsite-web)

## 项目介绍

[LearnSite](https://www.openlearnsite.com) 是一个专为中小学信息技术课堂设计的**开源教学辅助平台**。它通过整合新课教学、作业提交、作品互评、课堂测验及资源管理等功能，将教师从繁重的批改与统计中解放出来，让教学更聚焦于课堂本身。

平台支持**高度定制**：教师可按需开关功能模块、调整界面，甚至绑定学生IP实现精细化管理。同时，活跃的社区持续推动其进化——2026年初的更新已加入**网页课件活动、物联网/人工智能体验（如人脸识别、AI绘画）及在线协作工具**，紧跟技术前沿。

## 功能结构

### 平台架构
- **学生平台**：网站首页、我的课程、我的作品、常识测验、打字练习、资源下载
- **教师平台**：上课、备课、作品、签到、学生、测验、打字、资源、信息、状态
- **系统后台**：系统设置、班级设置、教师管理、新生导入、空间生成、学年升班、系统退出

### 核心特色
- **多平台支持**：Docker、Linux、Windows 全平台部署
- **丰富的教学工具**：编程学习、AI 体验、Scratch 积木编程、在线协作表格等
- **完整的评价体系**：作业互评、课堂表现、测验排行
- **智能管理**：学生 IP 绑定、自动签到、成绩统计

## 多平台部署指南

### Docker 部署

**快速启动**
```bash
# 使用 GitHub Actions 工作流构建的镜像
docker run -d --name learnsite \
  -p 8080:8080 \
  -e MONO_THREADS_PER_CPU=50 \
  ghcr.io/aymwoo/learnsite-wz:latest
```

**配合 MSSQL 数据库**
```bash
# 启动 MSSQL 容器
docker run -d --name learnsite-mssql \
  -e 'ACCEPT_EULA=Y' \
  -e 'MSSQL_SA_PASSWORD=YourStrong!Passw0rd' \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest

# 启动 LearnSite 容器（使用工作流构建的镜像）
docker run -d --name learnsite \
  -p 8080:8080 \
  --link learnsite-mssql:mssql \
  -e MONO_THREADS_PER_CPU=50 \
  ghcr.io/aymwoo/learnsite-wz:latest
```

**配置说明**
- **端口**: 8080 (XSP4 Web 服务器)
- **环境变量**: `MONO_THREADS_PER_CPU` (建议设置为 50)
- **数据持久化**: 可通过 `-v` 挂载卷保存数据

**使用 docker-compose 部署**

创建 `docker-compose.yml` 文件（使用工作流构建的镜像）：

```yaml
version: '3.8'

services:
  learnsite:
    # 使用 GitHub Actions 工作流构建的镜像
    image: ghcr.io/your-github-username/learnsite-wz:latest
    ports:
      - "8080:8080"
    environment:
      - MONO_THREADS_PER_CPU=50
    depends_on:
      - mssql
    restart: unless-stopped

  mssql:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - MSSQL_SA_PASSWORD=YourStrong!Passw0rd
    ports:
      - "1433:1433"
    volumes:
      - mssql_data:/var/opt/mssql
    restart: unless-stopped

volumes:
  mssql_data:
```

**注意**：将 `your-github-username` 替换为实际的 GitHub 用户名。例如，在 `RealKiro/learnsite-wz` 仓库中，镜像地址为 `ghcr.io/realkiro/learnsite-wz:latest`。

**启动命令**
```bash
docker-compose up -d
```

**停止命令**
```bash
docker-compose down
```

### Linux 部署

**1. 安装依赖**
```bash
# Ubuntu/Debian
sudo apt-get update
sudo apt-get install -y mono-complete mono-xsp4

# CentOS/RHEL
sudo yum install -y mono-complete mono-xsp4
```

**2. 部署步骤**
1. **下载发布包**
   ```bash
   wget https://github.com/RealKiro/learnsite-wz/releases/latest/download/learnsite-latest.zip
   unzip learnsite-latest.zip -d /var/www/learnsite
   ```

2. **配置权限**
   ```bash
   sudo chown -R www-data:www-data /var/www/learnsite
   sudo chmod +x /var/www/learnsite
   ```

3. **启动 XSP4 服务器**
   ```bash
   cd /var/www/learnsite
   xsp4 --port 8080 --address 0.0.0.0 --nonstop
   ```

4. **设置为系统服务** (可选)
   创建 `learnsite.service` 文件：
   ```ini
   [Unit]
   Description=LearnSite Web Application
   After=network.target

   [Service]
   WorkingDirectory=/var/www/learnsite
   ExecStart=/usr/bin/xsp4 --port 8080 --address 0.0.0.0 --nonstop
   Restart=always
   User=www-data

   [Install]
   WantedBy=multi-user.target
   ```

   ```bash
   sudo systemctl enable learnsite.service
   sudo systemctl start learnsite.service
   ```

### Windows 部署

**1. 安装依赖**
- **.NET Framework 4.8** (Windows 10/11 已内置)
- **IIS** (Internet Information Services)
- **SQL Server** (Express 版本即可)

**2. 部署步骤**
1. **下载发布包**
   从 GitHub Releases 下载最新的 `learnsite-latest.zip`

2. **解压到 IIS 目录**
   解压到 `C:\inetpub\wwwroot\learnsite`

3. **配置 IIS**
   - 打开 IIS 管理器
   - 创建新网站，指向 `C:\inetpub\wwwroot\learnsite`
   - 端口设置为 80 或其他可用端口
   - 应用池设置为 `.NET Framework v4.0`

4. **配置数据库**
   - 运行 `sql/learnsite.sql` 创建数据库
   - 修改 `web.config` 中的连接字符串

5. **启动网站**
   在 IIS 管理器中启动网站

### 数据库配置

**MSSQL 连接字符串**
```xml
<connectionStrings>
  <add name="LearnSiteConnectionString" 
       connectionString="Data Source=localhost;Initial Catalog=learnsite;User ID=sa;Password=YourStrong!Passw0rd;Encrypt=false;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**首次运行**
1. 访问 `http://localhost:8080` (Docker/Linux) 或 `http://localhost/learnsite` (Windows)
2. 系统会自动重定向到 `upgrade.aspx` 进行初始化
3. 按照提示完成数据库配置

## 相关资源

- **LearnSite 学习平台讨论 QQ 群**：5847120
- **官方网站**：[OpenLearnSite](https://www.openlearnsite.com/)
- **原作者**：[温州水乡 Github](https://github.com/WaterCountry/Learnsite)
- **代码仓库**：
  - [Gitee：LearnSite 主仓库源码](https://gitee.com/jnschool/learnsite-wz)
  - [Gitee：LearnSite 成都版分支源码](https://gitee.com/jnschool/game/tree/master/LearnSite_ChengDu)
  - [Github：learnsite-docker](https://github.com/RealKiro/learnsite-docker)
  - [Gitee：learnsite-docker (同步 Github 镜像)](https://gitee.com/realiy/learnsite-docker)
- **Docker 仓库**：
  - [learnsite-web](https://hub.docker.com/r/orzg/learnsite-web)
  - [mssql-learnsite](https://hub.docker.com/r/orzg/mssql-learnsite)
- **编译版**：
  - [Github：LearnSite 编译版](https://github.com/RealKiro/learnsite)
  - [Gitee：LearnSite 编译版 (同步 Github 镜像)](https://gitee.com/realiy/learnsite)

## 未来规划

- [ ] learnsite 3.0 + PostgreSQL 支持
- [ ] 更多 AI 教学工具集成
- [ ] 移动端适配优化
- [ ] 云服务部署方案

## 许可证

本项目采用 MIT 许可证，详见 [LICENSE](LICENSE) 文件。

## 核心贡献者

感谢所有为 LearnSite 项目做出贡献的开发者！

<div align="center">
  <table>
    <tr>
      <td align="center">
        <a href="https://github.com/WaterCountry">
          <img src="https://github.com/WaterCountry.png" width="100px;" alt="温州水乡"/>
          <br />
          <sub><b>温州水乡</b></sub>
        </a>
        <br />
        <sub>项目创始人</sub>
      </td>
      <td align="center">
        <a href="https://github.com/aymwoo">
          <img src="https://github.com/aymwoo.png" width="100px;" alt="aymwoo"/>
          <br />
          <sub><b>aymwoo</b></sub>
        </a>
        <br />
        <sub>Github 维护支持</sub>
      </td>
      <td align="center">
        <a href="https://github.com/RealKiro">
          <img src="https://github.com/RealKiro.png" width="100px;" alt="RealKiro"/>
          <br />
          <sub><b>RealKiro</b></sub>
        </a>
        <br />
        <sub>Docker 部署支持</sub>
      </td>
      <td align="center">
        <a href="https://gitee.com/jnschool">
          <img src="https://gitee.com/jnschool/avatar" width="100px;" alt="jnschool"/>
          <br />
          <sub><b>jnschool</b></sub>
        </a>
        <br />
        <sub>Gitee 维护支持</sub>
      </td>
    </tr>
  </table>
</div>

---

**LearnSite - 让信息技术教学更简单、更高效！**