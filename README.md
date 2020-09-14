
[![Build Status](https://dev.azure.com/MMarlonMs/MonteOlimpo/_apis/build/status/mmarlonms.monte-olimpo-auth-server)](https://dev.azure.com/MMarlonMs/MonteOlimpo/_build/latest?definitionId=1)

<img src="https://github.com/mmarlonms/monte-olimpo/blob/master/docs/monte-olimpo-logo.png" width="70" height="70">


# Monte Olimpo Auth Server

Este projeto apresenta uma visão simplificada de um sistema de autenticação para aplicação baseados em [JWT](https://jwt.io/) . O projeto se base em apresentar uma implementação **Genérica** de um servidor de Identidade que possa ser utilizado de forma fácil por diversas aplicações. O projeto possui implementação que facilita a customização das entidades do Identity o que deixa flexível para novas estruturas.

O projeto faz parte da projeto [Monte Olimpo](https://github.com/mmarlonms/monte-olimpo)  


## Estrutura

**MonteOlimpo.AuthServer** -
Api para autenticação dos usuários.

**MonteOlimpo.Identity.Abstractions** -
Implementação customizada das entidades base para utilização do [Microsoft Identity](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio) 

**MonteOlimpo.Identity.EntityFrameworkCore** -
Configuração do Contexto da aplicação, Migrations, Stores e Manager Customizados do [Microsoft Identity](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio) 

**MonteOlimpo.Authentication.JwtBearer** -
Provê serviços de Geração e Autenticação do Token baseados no Identity User


## Utilização
Para executar o projeto é necessário ter acesso a um banco de dados SQL server ou  PostGree e configura-lo no 
**appsettings.Development**

Caso prefira, basta executar o comando `docker-compose up` no projeto para criar o ambiente de desenvolvimento ideal com os seguintes componentes: 
 - Sql Server
 - Kibana
 - Email server

Obs.: É necessário ter o [Docker](https://docs.docker.com/docker-for-windows/install/) instalado 

## ![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=alert_status)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=sqale_index)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=code_smells)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
