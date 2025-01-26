# Mermaid diagrams


```Mermaid
graph TD
    subgraph Resource Group
        ACOP-Dev-FA[ACOP-Dev-FA<br>Function App<br>Executes serverless functions]
        ACOP-Dev-AI[ACOP-Dev-AI<br>Application Insights<br>Provides monitoring and diagnostics]
        ACOP-Dev-ASP[ACOP-Dev-ASP<br>App Service Plan<br>Hosts web applications]
        ACOP-Dev-KV[ACOP-Dev-KV<br>Key Vault<br>Manages secrets and keys]
        acopsa[acopsa<br>Storage Account<br>Stores unstructured data]
    end

    ACOP-Dev-FA --> ACOP-Dev-AI
    ACOP-Dev-FA --> ACOP-Dev-ASP
    ACOP-Dev-FA --> ACOP-Dev-KV
    ACOP-Dev-FA --> acopsa
```

