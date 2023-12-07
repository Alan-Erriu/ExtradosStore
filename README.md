# Extrados Store API

<div style="text-align: justify">

<hr/>

## Introducci�n

**Extrados Store** es una API De ecommerce donde los usuarios pueden registrarse, vender sus productos o comprar productos publicados por otros usuarios.


## Modelo de clases

Las clase que maneja Extrados Store para administrar sus usuarios son las siguientes:

#### 1. User

**Atributos**

|  Nombre                  |  Tipo        |
| :-----------------:      | :----:       |
|   user_id                | int pk       |
|   user_name              | string       |
|   user_lastname          | string       |
|   user_email             | string       |
|   user_phone_number      | string       |
|   user_password_hash     | string       |
|   user_roleid            | (fk role)    |
|   user_status            | bool         |
|   user_created_at        | long(epoch)  |
|   user_date_of_birth     | long (epoch) |

#### 2. Role
En este modelo solo extisten dos roles: "user" "admin"

**Atributos**

| Nombre             |  Tipo  |
| :---------------:  | :----: |
|  role_id           | int pk |
|  role_name         | string |
|  role_description  | string |

#### 3. Token
Este modelo contiene los datos del refresh token

**Atributos**

|               Nombre                  |  Tipo        |
|     :-----------------------------:   | :----:       |
|  token_id                             | int pk       |
|  token_userid                         | int pk       | 
|  token_accesstoken                    | string       |
|  token_refreshToken                   | string       |
|  token_expiration_date_refreshtokenn  | long (epoch) |

<hr />


## Endpoints 

La API cuenta con 3 tipos de endpoints: 
**Auth** que corresponde a iniciar sesion,tokens y registrarse.
**Role** que corresponde a las acciones relacionadas con los roles (crear y obtener info)
**User** que corresponde a todo lo que tenga que ver con operaciones crud del usuario (algunas solo se puede acceder siendo admin)
Casi todos los endpoints necesitan un token en la cabezera para funcionar, de no recibirlo se obtendra un Unauthorize(401)



#### 1 (Auth)

#### 1 Crear un usuario/registrase

   ##### `POST /api/Auth/signup`

En el body de la request:

```json

{
  "user_name": "Lucas Ezquiel",
  "user_lastname": "Erriu",
  "user_email": "luas@gmail.com",
  "user_password_hash": "123456",  
  "user_date_of_birth": "1989-07-16T00:00:00.093Z",
  "user_phone_number":"+541167659472"
}

````

##### Respuestas

|   Caso    | Status |             Respuesta                    |
| :-------: | :----: | :--------------------------------:       |
|   Exito   |  200   |          { "succes" }                    |
| Not Found |  404   |     { "*user* role not found" }          |
| Conflict  |  409   | { "The phone number is already in use" } |
| Conflict  |  409   |      { "The email is already in use" }   |
|   Fallo   |  500   |          { "Server error" }              |


Si los datos del cuerpo de la request estan correctos, se creara el usuario en la base de datos con un id auto incrementado y rol_id 1 (user) fk con role_id.


#### 2 Iniciar sesion

   ##### `POST /api/Auth/signin`

En el body de la request:

```json

{   
    "user_email": "jorgelina0273@gmail.com",
    "user_password": "123456"
}

````

##### Respuestas

|   Caso       | Status |             Respuesta             |
| :-------:    | :----: | :--------------------------------:|
|   Exito      |  200   |    { refresh y access token }     |
| Unauthorized |  401   |     { "incorrect password" }      |
| Unauthorized |  401   |    { "The user is disabled" }     |
|   Fallo      |  500   |       { "Server error" }          |


Si los datos del cuerpo de la request estan correctos, se devolvera las crendeciales de accesso (refresh y access token)



#### 3 Obtener tokens

   ##### `POST /api/Auth/gettoken`

En el body de la request:

```json

{

    "AccessToken":"***********",
    "refreshToken":"**********"
}

````

##### Respuestas

|   Caso       | Status |             Respuesta             |
| :-------:    | :----: | :--------------------------------:|
|   Exito      |  200   |    { refresh y access token }     |
| Unauthorized |  401   |     { " invalid token" }          |
| Unauthorized |  401   |    { "invalid refresh token" }    |
| Unauthorized |  401   |    { "invalid access token" }     |
| Unauthorized |  401   | { "Invalid access token format" } |
|   Fallo      |  500   |       { "Server error" }          |


Si los datos del cuerpo de la request estan correctos, se devolvera las nuevas crendeciales de accesso (refresh y access token).


#### 2 (Role)

#### 1 Crear un rol. Requiere rol admin

   ##### `POST /api/Role/create`
Esta se creo unicamente con la idea de no insertar los roles de manera manual en la bd.
Las necesidades de la api requieren solo un rol por usuario, pero se puede crear una tabla extra y asociar usuarios con roles. Para tener mas de un rol por usuario.


En el body de la request:

```json

{

       "name_role":"user",
       "description_role":"funciones basicas: crear una publicacion etc..."    

}

````

##### Respuestas

|   Caso       | Status |             Respuesta                 |
| :-------:    | :----: | :--------------------------------:    |
|   Exito      |  200   |              { "succes"  }            |
|   Forbidden  |  403   |                                       |
| Unauthorized |  401   |                                       |
| Conflict     |  409   | { " The name role is already in use" }|
|   Fallo      |  500   |       { "Server error" }              |  


#### 2 Obtener todos los roles y la info. Requiere rol admin

##### `GET /api/Role/getroles`



##### Respuestas

|   Caso       | Status |             Respuesta                 |
| :-------:    | :----: | :--------------------------------:    |
|   Exito      |  200   |        { lista de roles  }            |
| Unauthorized |  401   |                                       |
|   Forbidden  |  403   |                                       |
|   Fallo      |  500   |       { "Server error" }              |

Devuelve los roles


#### 3 (User)

#### 1 Deshabilita un usuario. Requiere rol admin

   ##### `PUT /api/User/disable/userId`


|   Caso       | Status |      Respuesta         |
| :-------:    | :----: | :---------------------:|
|   Exito      |  200   |   { "user disable"  }  |
| Unauthorized |  401   |                        |
|   Forbidden  |  403   |                        |
|   Not Found  |  404   |   {"user not found"}   |
|   Fallo      |  500   |   { "Server error" }   |

Devuelve los roles