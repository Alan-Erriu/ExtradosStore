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
|   long user_created_at   | long(epoch)  |
|   user_date_of_birth     | long (epoch) |

#### 3. Role
En este modelo solo extisten dos roles: "user" "admin"

**Atributos**

| Nombre             |  Tipo  |
| :---------------:  | :----: |
|  role_id           | int pk |
|  role_name         | string |
|  role_description  | string |

<hr />


## Endpoints

#### Acciones de usuario (con rol user)



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
