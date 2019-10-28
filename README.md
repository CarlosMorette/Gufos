# GUFOS - Agenda de Eventos - BackEnd C# - .Net Core 3.0

## Requisitos
> - Visual Studio Code <br>
> - Banco de Dados funcionando - DDLs, DMLs e DQLs <br>
> - .NET Core SDK 3.0

## Cria��o do Projeto
> Criamos nosso projeto de API com: 
```bash
dotnet new webapi
```
<br>

## Entity Framework - Database First

> Instalar o gerenciador de pacotes EF na m�quina:
```bash
dotnet tool install --global dotnet-ef
```

<br>

> Baixar Pacote SQL Server:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

<br>

> Baixar pacote de escrita de c�digos do EF:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

<br>

> Dar um restore na aplica��o para ler e aplicar os pacotes instalados:
```bash
dotnet restore
```

<br>

> Testar se o EF est� ok
```bash
dotnet ef
```

<br>

> Criar os Models � partir da sua base de Dados
    :point_right: -o = criar o diretorio caso n�o exista
    :point_right: -d = Incluir as DataNotations do banco
```bash
dotnet ef dbcontext scaffold "Server=DESKTOP-XVGT587\SQLEXPRESS;Database=Gufos;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -d
```
<br>

## Controllers
> Apagamos o controller que j� vem com a base...

### CategoriaController

> Criamos nosso primeiro Controller: CategoriaController <br>
> Herdamos nosso novo controller de ControllerBase <br>
> Definimos a "rota" da API logo em cima do nome da classe, utilizando:
```c#
[Route("api/[controller]")]
```
> Logo abaixo dizemos que � um controller de API, utilizando:
```c#
[ApiController]
```
<br>

> Damos **CTRL + .** para incluir:

```c#
using Microsoft.AspNetCore.Mvc;
```
<br>

> Instanciamos nosso contexto da nossa Base de Dados:
```c#
GufosContext _contexto = new GufosContext();
```

> Damos **CTRL + .** para incluir nossos models:
```c#
using GUFOS_BackEnd.Models;
```

> Criamos nosso m�todo **GET**:
```c#
        // GET: api/Categoria/
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            var categorias = await _context.Categoria.ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }
```

> Importamos com CTRL + . as depend�ncias:
```c#
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
```
<br>

> Testamos o m�todo GET de nosso controller no Postman:
```bash
dotnet run
https://localhost:5001/api/categoria
```

> Deve ser retornado:
```json
[
    {
        "categoriaId": 1,
        "titulo": "Desenvolvimento",
        "evento": []
    },
    {
        "categoriaId": 2,
        "titulo": "HTML + CSS",
        "evento": []
    },
    {
        "categoriaId": 3,
        "titulo": "Marketing",
        "evento": []
    }
]
```

<br>

> Criamos nossa sobrecarga de m�todo **GET**, desta vez passando como par�metro o ID:
```c#
        // GET: api/Categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }
```
> Testamos no Postman: [https://localhost:5001/api/categoria/1](https://localhost:5001/api/categoria/1)

<br>

> Criamos nosso m�todo **POST** para inserir uma nova categoria:
```c#
        // POST: api/Categoria/
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            try
            {
                await _context.AddAsync(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return categoria;
        }
```
> Testamos no Postman, passando em RAW , do tipo JSON:
```json
{
    "titulo": "Teste"
}
```

<br>

> Criamos nosso m�todo **PUT** para atualizar os dados:
```c#
        // PUT: api/Categoria/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var categoria_valido = await _context.Categoria.FindAsync(id);

                if (categoria_valido == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
```
> Testamos no Postman, no m�todo PUT, pela URL [https://localhost:5001/api/categoria/4](https://localhost:5001/api/categoria/4) passando:
```json
{
    "categoriaId": 4,
    "titulo": "Design Gr�fico"
}
```

<br>

> Por �ltimo, inclu�mos nosso m�todo **DELETE** , para excluir uma determinada Categoria:
```c#
        // DELETE: api/Categoria/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }
```
> Testamos pelo Postman, pelo m�rodo DELETE, e com a URL: [https://localhost:5001/api/categoria/4](https://localhost:5001/api/categoria/4) <br>
> Deve-se retornar o objeto exclu�do:
```json
{
    "categoriaId": 4,
    "titulo": "Design Gr�fico",
    "evento": []
}
```

<br>

### LocalizacaoController
> Copiar ControllerCategoria e alterar com **CTRL + F** <br>
> Testar os m�todos REST

<br>

### EventoController
> Copiar ControllerCategoria e alterar com **CTRL + F** <br>
> Testar os m�todos REST <br>
> Notamos que no m�todo **GET** n�o retorna a �rvore de objetos *Categoria* e *Localizacao* <br>
> Para incluirmos � necess�rio adicionar em nosso projeto o seguinte pacote:
```bash
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
```
> Depois em nossa Startup.cs, dentro de ConfigureServices, no lugar de *services.AddControllers()* :
```c#
services.AddControllersWithViews().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
```
> Damos **CTRL + .** para incluir a depend�ncia:
```c#
using Newtonsoft.Json;
```
> Ap�s isso precisamos mudar nosso controller para receber os atributos, no m�todo GET ficar� assim:
```c#
var eventos = await _context.Evento.Include(c => c.Categoria).Include(l => l.Localizacao).ToListAsync();
```
> No m�todo GET com par�metro ficar� assim:
```c#
var evento = await _context.Evento.Include(c => c.Categoria).Include(l => l.Localizacao).FirstOrDefaultAsync(e => e.EventoId == id);
```

<br>

> Adicionar os Controllers restantes

<br><br>

## SWAGGER - Documenta��o da API

>  Instalar o Swagger:
```bash
dotnet add Gufos_BackEnd.csproj package Swashbuckle.AspNetCore -v 5.0.0-rc4
```
<br>

> Registramos o gerador do Swagger dentro de ConfigureServices, definindo 1 ou mais documentos do Swagger:
```c#
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                // Mostrar o caminho dos coment�rios dos m�todos Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
```

<br>

> Colocar na Startup com **CTRL + .**
```c#
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
```

<br>

> Colocar dentro do csproj para gerar a documenta��o com base nos coment�rios dos m�todos:
```c#
  <PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>
```

<br>

> Em Startup.cs , no m�todo Configure, habilite o middleware para atender ao documento JSON gerado e � interface do usu�rio do Swagger:
```c#
            // Habilitamos efetivamente o Swagger em nossa aplica��o.
            app.UseSwagger();
            // Especificamos o endpoint da documenta��o
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
```

<br>

> Rodar a aplica��o e testar em: [https://localhost:5001/swagger/](https://localhost:5001/swagger/)

<br>

## JWT - Autentica��o com Json Web Token

> Instalar pacote JWT
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.0.0
```
<br>

> Adicionar a configura��o do nosso Servi�o de autentica��o:
```c#
            // JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
            .AddJwtBearer(options =>  
            {  
                options.TokenValidationParameters = new TokenValidationParameters  
                {  
                    ValidateIssuer = true,  
                    ValidateAudience = true,  
                    ValidateLifetime = true,  
                    ValidateIssuerSigningKey = true,  
                    ValidIssuer = Configuration["Jwt:Issuer"],  
                    ValidAudience = Configuration["Jwt:Issuer"],  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))  
                };  
            });
```

> Importar com **CTRL + .** as depend�ncias:
```c#
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
```

<br>

> Adicionamos em nosso *appsettings.json* :
```json
{
  "Jwt": {  
    "Key": "GufosSecretKey",  
    "Issuer": "gufos.com"  
  },
}  
```

<br>

> Em Startup.cs , no m�todo Configure , usamos efetivamente a autentica��o:
```c#
app.UseAuthentication();
``` 

<br><br>

> Criamos o Controller *LoginController* e herdamos da *ControllerBase* <br>
> Colocamos a rota da API e dizemos que � um controller de API :
```c#
    [Route("api/[controller]")]  
    [ApiController] 
    public class LoginController : ControllerBase
    {
        
    }
```
<br>

> Criamos nossos m�todos:
```c#
        // Chamamos nosso contexto do banco
        GufosContext _context = new GufosContext();

        // Definimos uma vari�vel para percorrer nossos m�todos com as configura��es obtidas no appsettings.json
        private IConfiguration _config;  

        // Definimos um m�todo construtor para poder passar essas configs
        public LoginController(IConfiguration config)  
        {  
            _config = config;  
        }

        // Chamamos nosso m�todo para validar nosso usu�rio da aplica��o
        private Usuario AuthenticateUser(Usuario login)  
        {  
            var usuario =  _context.Usuario.FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
  
            if (usuario != null)  
            {  
                usuario = login;  
            }  

            return usuario;  
        }  

        // Criamos nosso m�todo que vai gerar nosso Token
        private string GenerateJSONWebToken(Usuario userInfo)  
        {  
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Definimos nossas Claims (dados da sess�o) para poderem ser capturadas
            // a qualquer momento enquanto o Token for ativo
            var claims = new[] {  
                new Claim(JwtRegisteredClaimNames.NameId, userInfo.Nome),  
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  
            }; 

            // Configuramos nosso Token e seu tempo de vida
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],  
              _config["Jwt:Issuer"],  
              claims,  
              expires: DateTime.Now.AddMinutes(120),  
              signingCredentials: credentials);  
  
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }  
  

        
        // Usamos essa anota��o para ignorar a autentica��o neste m�todo, j� que � ele quem far� isso  
        [AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login([FromBody]Usuario login)  
        {  
            IActionResult response = Unauthorized();  
            var user = AuthenticateUser(login);  
  
            if (user != null)  
            {  
                var tokenString = GenerateJSONWebToken(user);  
                response = Ok(new { token = tokenString });  
            }  
  
            return response;  
        }
```

> Importamos as depend�ncias:
```c#
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using GUFOS_BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
```
<br>

> Testamos se est� sendo gerado nosso Token pelo Postman, no m�todo POST <br>
> Pela URL : [https://localhost:5001/api/login](https://localhost:5001/api/login) <br>
> E com os seguintes par�metros pela RAW : 
```json
{
    "nome": "Administrador",
    "email": "adm@adm.com",
    "senha": "123",
}
```

> O retorno deve ser algo do tipo:
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJQYXVsbyIsImVtYWlsIjoiYWRtQGFkbS5jb20iLCJqdGkiOiIwYjNmMGM3ZC1mMDhjLTQ4NDQtOTI1Mi04ZDI1ZTZmY2MxYmYiLCJleHAiOjE1NzExNTcyMjUsImlzcyI6Imd1Zm9zLmNvbSIsImF1ZCI6Imd1Zm9zLmNvbSJ9.bk_cvQJgVpq7TXa8Nhh1XzWAEUnTXHc2lP5vvqIVhJs"
}
```

> Ap�s confirmar, vamos at� [https://jwt.io/](https://jwt.io/) <br>
> Colamos nosso Token l� e em Payload devemos ter os seguintes dados:
```json
{
  "nameid": "Administrador",
  "email": "adm@adm.com",
  "jti": "d1e13b73-5f8f-423c-97e2-835f55bbfb0e",
  "exp": 1571157573,
  "iss": "gufos.com",
  "aud": "gufos.com"
}
```

<br><br>

> Pronto! Agora � s� utilizar a anota��o *[Authorize]* em baixo da anota��o REST de cada m�todo que desejar colocar autentica��o! <br>
> No Postman devemos gerar um token pela rota de login e nos demais endpoints devemos adicionar o token gerado na aba *Authorization*  escolhendo a op��o ***Baerer Token***





