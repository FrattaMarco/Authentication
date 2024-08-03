PREMESSA: metà della logica presente in questa api si riferisce ad un contesto docker. I vari docker compose file e docker file NON sono presenti all'interno del repository: nel caso in cui vogliate far
girare il tutto in un ambiente docker, vi basta creare\configurare i vostri docker file\docker compose file ed i certificati dato che poi il resto è configurato lato codice.
Inoltre tutto ciò è stato creato con lo scopo di dare un'idea basilare di ASP NET CORE nel mio poco tempo libero a disposizione tra un week end e l'altro.

Questa API è stata pensata e sviluppata per produrre un JWT di autenticazione. Infatti, inviando le credenziali all'endpoint, se esse fossero corrette verrebbe prodotto il token di autenticazione. E' stato
utilizzato il CQRS pattern. Inoltre, i log in un ambiente "development" vengono scritti in un file di testo e sulla console mentre in un ambiente docker vengono scritti sulla console ed all'interno di Elastic
(in modo tale da vere un'ottima visuale dei log tramite Kibana). 

La parte ELK NON è compresa nel repository (quindi non ci sono i docker file ed il relativo docker compose file) tuttavia vi rimando alla pagina ufficiale ELK dove vi è la guida per implementare i container
inerenti ad Elastic e Kibana

https://www.elastic.co/guide/en/elasticsearch/reference/7.17/configuring-tls-docker.html

Argomenti chiave trattati:
#CQRS Pattern
#Docker
#.Net 8
#Health Checks
#Swagger
#Api versioning
#JWT Authentication
#Serilog
#Fluent Validation
#Generic Repository Pattern
#MediatR
#Dapper e SSMS
