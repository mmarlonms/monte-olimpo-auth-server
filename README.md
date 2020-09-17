

[![Build Status](https://dev.azure.com/MMarlonMs/MonteOlimpo/_apis/build/status/mmarlonms.monte-olimpo-auth-server)](https://dev.azure.com/MMarlonMs/MonteOlimpo/_build/latest?definitionId=1)

<img src="https://github.com/mmarlonms/monte-olimpo/blob/master/docs/monte-olimpo-logo.png" width="70" height="70">


# Monte Olimpo Auth Server

Este projeto apresenta uma visão simplificada de um sistema de autenticação para aplicação baseados em [JWT](https://jwt.io/) . O projeto se base em apresentar uma implementação **Genérica** de um servidor de Identidade que possa ser utilizado de forma fácil por diversas aplicações. O projeto possui implementação que facilita a customização das entidades do Identity o que deixa flexível para novas estruturas.

O projeto faz parte da projeto [Monte Olimpo](https://github.com/mmarlonms/monte-olimpo)  


## Estrutura

**MonteOlimpo.AuthServer** -
API para autenticação dos usuários.

**MonteOlimpo.Identity.Abstractions** -
Implementação customizada das entidades base para utilização do [Microsoft Identity](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio) 

**MonteOlimpo.Identity.EntityFrameworkCore** -
Configuração do contexto da aplicação, migrations, stores e manager customizados do [Microsoft Identity](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio) 

**MonteOlimpo.Authentication.JwtBearer** -
Provê serviços de geração e autenticação do token baseados no Identity User

## Utilização
Para executar o projeto é necessário ter acesso a um banco de dados SQL Server ou  PostGresql e configura-lo no 
**appsettings.Development**

Caso prefira, basta executar o comando `docker-compose up` no projeto para criar o ambiente de desenvolvimento ideal com os seguintes componentes: 
 - SQL Server
 - Elastic Search
 - Kibana
 - Email server

Obs.: É necessário ter o [Docker](https://docs.docker.com/docker-for-windows/install/) instalado 

## Logs

Caso tenha optado por utilizar o elastic seach como repositório de logs teremos o seguinte resultado: 

![enter image description here](https://github.com/mmarlonms/monte-olimpo-auth-server/blob/master/docs/Kibana.png?raw=true)

1. Endereço do Kibana, no caso estou usando o docker local para executa-lo. 
2. Últimas atualizações
3. Índice da aplicação
4. Time Line de log e último log registrado
5. Campo customizado enviado da aplicação, no meu caso **userName**

Para maiores informações sobre como configurar o log na aplicação utilizando o _**MonteOlimpo.Base.Log**_ acesse o link a baixo:

 - https://github.com/mmarlonms/monte-olimpo-base

Para ver mais sobre a configuração do kibana e elasticseach  sugiro os seguintes links: 

 - [Configurando o Elasticsearch e Kibana no  Docker](https://medium.com/@hgmauri/configurando-o-elasticsearch-e-kibana-no-docker-3f4679eb5feb)
   
 -  [ Criando Index Pattern no kibana](https://www.youtube.com/watch?v=2ZB81TcQAnw)

## ![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=alert_status)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=sqale_index)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=mmarlonms_monte-olimpo-auth-server&metric=code_smells)](https://sonarcloud.io/dashboard?id=mmarlonms_monte-olimpo-auth-server)
