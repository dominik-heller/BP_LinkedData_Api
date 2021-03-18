<article><h1>LinkedDataApi</h1>
<p>REST API for extraction and manipulation with semantic data, normally available only via SPARQL endpoints.</p>
<p>This project is part of bachelor thesis <strong><em>Linked data utilization via web API</em></strong> at Faculty of Science, The University of South Bohemia, České Budějovice, Czech Republic.</p>
</article>
<article><h2>Docker support</h2>
<p>LinkedDataApi is also available as standalone <a href='https://hub.docker.com/r/helldo/linkeddataapi'>Docker image</a>.
<h3>Docker-compose</h3>
<p>Alternatively, if you want to test UPDATE capabilties, use following <em>docker-compose</em> configuration to set up dedicated sparql server instance alongside LinkedDataApi.</p>
<pre><code>
version: "3.9"
services:
    virtuoso:
        image: "helldo/virtuoso"
        container_name: "virtuoso"
        ports:
            - "8890:8890"
        networks:
            - virtuoso
    api:
        image: "helldo/linkeddataapi"
        container_name: "api"
        ports:
            - "8080:80"
        networks:
        - virtuoso
networks:    
    virtuoso:
        driver: bridge</code></pre>
<p>Copy the configuration into new file called <i>docker-compose.yml</i> and run cli commands "docker-compose build" and "docker-compose up". 
LinkedDataApi will be available at <i>localhost:8080</i> and sparql endpoint at <i>localhost:8890/sparql</i>. 
Server contains predefined datasets which can be accessed and manipulated via endpointName "virtuoso", eg. <i>http://localhost:8080/api/virtuoso...</i> . 
Additionaly you can even modify the sparql server configuration at <i>localhost:8890</i>, ie. add datasets, create new named graphs etc.
</p>
</article>
