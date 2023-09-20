FROM docker.repo1.uhc.com/ibmcom/db2:11.5.8.0

RUN mkdir /var/custom

COPY ./database/DB2/entrypoint.sh /var/custom
COPY ./database/DB2/*.sql /
COPY ./database/DB2/01-db2.init.sh /

RUN chmod a+x /var/custom/entrypoint.sh
RUN chmod a+x /01-db2.init.sh