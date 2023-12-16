# Extrados Store API

<div style="text-align: justify">

<hr/>

## Introducci�n

**Extrados Store** es una API De ecommerce donde los usuarios pueden registrarse, vender sus productos o comprar productos publicados por otros usuarios.


## Capas

El proyecto esta estructurado de forma que api consume a servicios y este a daos.
La capa config contiene informacion sensible neceseria para jwt y db
La capa common es el lugar donde se ubica todo aquello que necesitamos en varias capas como las custom exceptions, las request y demas...
La capa entities contien los modelos y los DTOs

 ```
.

├── ExtradoStore
│   ├── api
│   │   └──
│   ├── services
│   │   └── ... 
│   ├── data
│   │   └── ...
│   ├── config
│   │   └── ...
│   ├── common
│   │   └── ...
│   ├── entities
│   │   └── ... 

```


## Modelo de clases

Las clases que maneja Extrados Store para administrar sus usuarios son las siguientes:

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

### 4. Post


**Atributos**

|  Nombre                  |  Tipo                 |
| :-----------------:      | :----:                |
|   post_id                | int pk                |
|   post_userid            | int   (fk_user)       |
|   post_name              | string                |
|   post_description       | string                |
|   post_price             | decimal               |
|   post_stock             | int                   |
|   post_img               | byte[]                |
|   post_status_id         | int (fk_post_status)  |
|   post_categoryId        | long(fk_category)     |
|   post_create_at         | long (epoch)          |
|   post_brandIdt          | int (fk_brand)        |


### 5. PostStatus

**Atributos**

|  Nombre                  |  Tipo    |
| :-----------------:      | :----:   |
|   post_status_id         | int pk   |
|   post_status_name       | string   |

### 6. Brand

**Atributos**

|  Nombre        |  Tipo    |
| :------------: | :----:   |
|   brand_id     | int pk   |
|   brand_name   | string   |

### 7. Category

**Atributos**

|  Nombre             |  Tipo    |
| :-----------------: | :----:   |
|   category_id       | int pk   |
|   categoty_name     | string   |

### 8. Offer

**Atributos**

|  Nombre                 |  Tipo  |
| :-----------------:     | :----: |
|  offer_id               | int pk |
|  offer_name             | string |
|  offer_date_start       | long   |
|  offer_date_expiration  | long   |
|  offer_status           | bool   |

### 9. OfferPost

**Atributos**

|  Nombre              |  Tipo        |
| :-----------------:  | :----:       |
| offer_post_id        | int pk       |
| offer_post_postId    | int fk_post  |
| offer_post_offerId   | int fk_offer |
| offer_post_discount  | int          |
| offer_post_status    | bool         |




<hr />


## Endpoints 

La API cuenta con 3 tipos de endpoints: 
<n/>
**Auth** que corresponde a iniciar sesion,tokens y registrarse.
<n/>
**Role** que corresponde a las acciones relacionadas con los roles (crear y obtener info)
<n/>
**User** que corresponde a todo lo que tenga que ver con operaciones crud del usuario (algunas solo se puede acceder siendo admin)
<n/>
**category** crear categorias para las publicaciones
<n/>
**brand** creat marcas para las publicaciones
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


|   Caso       | Status |      Respuesta                 |
| :-------:    | :----: | :---------------------:        |
|   Exito      |  200   |   { "user disable"  }          |
| Unauthorized |  401   |                                |
|   Forbidden  |  403   |                                |
|   Not Found  |  404   |   {"user not found"}           |
| Conflict     |  409   |{"the user was already disable"}|
|   Fallo      |  500   |   { "Server error" }           |

Caso exitoso cambia el status del usuario a false

#### 2 Habilita un usuario. Requiere rol admin

   ##### `PUT /api/User/enable/userId`


|   Caso       | Status |      Respuesta                 |
| :-------:    | :----: | :---------------------:        |
|   Exito      |  200   |   { "user enable"  }           |
| Unauthorized |  401   |                                |
|   Forbidden  |  403   |                                |
|   Not Found  |  404   |   {"user not found"}           |
| Conflict     |  409   |{"the user was already enable"} | 
|   Fallo      |  500   |   { "Server error" }           |

Caso exitoso cambia el status del usuario a true

#### 3 Pasa el rol de un usuario de user a admin. Requiere rol admin

   ##### `PUT /api/User/uprgrade/userId`


|   Caso       | Status |      Respuesta                                       |
| :-------:    | :----: | :---------------------:                              |
|   Exito      |  200   |   { "now user is admin"  }                           |
| Unauthorized |  401   |                                                      |
|   Forbidden  |  403   |                                                      |
|   Not Found  |  404   |   {"role *admin* not found"}                         |
| Conflict     |  409   |{"The user's role was already admin in the database"} | 
|   Fallo      |  500   |   { "Server error" }                                 |

Caso exitoso cambia el status del usuario a true


#### 4 (Category)

Las publicaciones estan enlazadas a una cetegoria mediante un id. Por ejemplo zapatillas adidas formaría parte de la categoria zapatillas. 

#### 1 Crear categoria

  ##### `POST /api/Auth/gettoken` 
 
En el body de la request, (requiere admin role)


```json

{
    "category_name":"zapatillas"
}

````

|   Caso       | Status |      Respuesta                        |
| :-------:    | :----: | :---------------------------------:   |
|   Exito      |  200   | { "category created" }                |
| Unauthorized |  401   |                                       |
|   Forbidden  |  403   |                                       |
|   Conflict   |  409   | {"name category is already in use"}   |
|   Fallo      |  500   |   { "Server error" }                  |


 #### 2 Borrar una categoría por id
  
  #####  `DELETE /api/Category/delete/(idcategory)`

  No se puede eliminar una categoria que ya este enlazada a una publicacion, este endpoint es por si se creo una categoria erronea o no es necesaria.
  (requiere admin role)

|   Caso       | Status |                     Respuesta                                       |
| :-------:    | :----: | :---------------------------------------------------------:         |
|   Exito      |  200   | { "category deleted" }                                              |
| Unauthorized |  401   |                                                                     |
|   Forbidden  |  403   |                                                                     |
| Not found    |  404   |  {"id category not found"}                                          |
| Bad request  |  400   |  {"You cannot delete a category associated with an existing post"}  |
|   Fallo      |  500   |   { "Server error" }                                                |



 #### 3 Obtener la lista de categorias creadas
  
  #####  `GET /api/Category/getcategorys`

|   Caso       | Status |     Respuesta              |
| :-------:    | :----: | :----------------------:   |
|   Exito      |  200   | { [all categgorys] }       |
| Unauthorized |  401   |                            |
|   Forbidden  |  403   |                            |
|   Fallo      |  500   |   { "Server error" }       |


#### 5 (Brand)

Todas las publicaciones estan enlazadas a una marca, se necesita ser administrador para manejar el crud.

####

#### 1 Crear brand 

  ##### `POST /api/brand/create` 
 
En el body de la request, (requiere admin role)


```json

{
    "brand_name":"adidas"
}

````

|   Caso       | Status |      Respuesta                        |
| :-------:    | :----: | :---------------------------------:   |
|   Exito      |  200   | { "brand created" }                   |
| Unauthorized |  401   |                                       |
|   Forbidden  |  403   |                                       |
|   Conflict   |  409   | {"the name brand is already in use"}  |
|   Fallo      |  500   |   { "Server error" }                  |


#### 2 Obtener array de todos los brand 

  ##### `GET /api/Brand/getbrands`

|   Caso       | Status |     Respuesta        |
| :-------:    | :----: | :-------------------:|
|   Exito      |  200   | { [all brands] }     |
| Unauthorized |  401   |                      |
|   Forbidden  |  403   |                      |
|   Fallo      |  500   | { "Server error" }   |



 #### 3 Borrar una marca por id
  
  #####  `DELETE /api/Brand/delete/(idcategory)`

  No se puede eliminar una marca que ya este enlazada a una publicacion, este endpoint es por si se creo una marca erronea o no es necesaria.
  (requiere admin role)

|   Caso       | Status |                     Respuesta                                   |
| :-------:    | :----: | :---------------------------------------------------------:     |
|   Exito      |  200   | { "brand deleted" }                                             |
| Unauthorized |  401   |                                                                 |
|   Forbidden  |  403   |                                                                 |
| Not found    |  404   |  {"id category not found"}                                      |
| Bad request  |  400   |  {"You cannot delete a brand associated with an existing post"} |
|   Fallo      |  500   |   { "Server error" }                                            |


#### 6 (Post)

####

#### 1 Crear un post

  ##### `POST /api/Post/create` 

Para crear un post hay que mandar un id de brand y uno de categoría si o si.

 
En el body de la request


```json

{

    "post_name": "zapatillas",
    "post_description": "las mas rapidas del oeste",
    "post_price": 12000.99,
    "post_stock": 10,
    "category_id":1,
    "brand_id":1
  
}

````

|   Caso       | Status |      Respuesta                        |
| :-------:    | :----: | :---------------------------------:   |
|   Exito      |  200   | { "post  created" }                   |
| Unauthorized |  401   |                                       |
|   Fallo      |  500   |   { "Server error" }                  |



#### 2 Pausar Una publicacion

  ##### `Put /api/Post/statuspaused/(postId)` 

Los clientes con rol usuario solo podran pausar sus propios post, el admin puede pausar cualquiera

 



|   Caso       | Status |      Respuesta                        |
| :-------:    | :----: | :---------------------------------:   |
|   Exito      |  200   | { "brand created" }                   |
| Unauthorized |  401   |                                       |
|   Forbidden  |  403   |                                       |
| NotFound     |  404   | { "status not found"}                 |
|   Fallo      |  500   |   { "Server error" }                  |


#### 3 Cancelar Una publicacion

  ##### `Put /api/Post/statuscancelled/(postId)` 

Solo el admin puede setear el estado de una publicacion en "cancelled" tabla post_status


|   Caso       | Status |      Respuesta                    |
| :-------:    | :----: | :---------------------:           |
|   Exito      |  200   | { "post status now is cancelled" }|
| Unauthorized |  401   |                                   |
|   Forbidden  |  403   |                                   |
| NotFound     |  404   | { "status not found"}             |
|   Fallo      |  500   |   { "Server error" }              |


#### 4 Activar un post

  ##### `Put /api/Post/statusactive/(postId)/newStock` 

Los usuarios solo pueden activar sus propios post (incluyendo al admin) el admin no puede activar publicaciones de otros usuarios.
Tambien es necesario mandar en la url el nuevo stock (int), es decir que tambien actualiza el stock

|   Caso       | Status |      Respuesta                    |
| :-------:    | :----: | :---------------------:           |
|   Exito      |  200   | {"post status now is active" }    |
| Unauthorized |  401   |                                   |
| NotFound     |  404   | { "status not found"}             |
|   Fallo      |  500   |   { "Server error" }              |


#### 5 Actualizar/cambiar datos de un post

  ##### `Put /api/Post/update` 
Atencion, este endpoint funciona de forma dinamica, de no recibir un campo, el mismo conservara los valores anteriores.
Tanto los id como precio y stock. En caso de no agregarse al body o agregarse con valor 0 no se consideraran como nuevos valores.

PD: Claramente hay que agregar id existentes.

  En el body de la request


```json

{

     "postId":2,
     "postName":"adidas running t2000",
     "postDescription":"zapatatillas un poco usadas, pero andan",
     "postPrice":0,
     "postStock":2,
     "postBrandId":2,
     "postCategoryId":0
  
}

````


|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"post status now is active" }                   |
| Unauthorized |  401   |                                                  |
| NotFound     |  404   | { "status not found"}                            |
| BadRequest   |  400   | { "Post stock must have a valid positive value "}|
| BadRequest   |  400   | { "Post price must have a valid positive value "}|
|   Fallo      |  500   |   { "Server error" }                             |

#### 7 (PostStatus)

####

#### 1 Falta implementar los endpoints que hagan un crud de post_status. :(


#### 8 (Offer)

####


#### 1 Crear una oferta


  ##### `Post /api/Offer/create`
Atencion, las fechas  deben ir con su huso horario correspondiente al pais ej: (-03 al final para argentina).
Todas las fechas de la aplicacion se pasan a utc y en milisegundos en la db (epoch).



  En el body de la request


```json

{

   "offer_name": "ofertas razer por alfonsin",
   "offer_date_start": "2023-12-16T15:59:00.093-03",
   "offer_date_expiration": "2023-12-15T16:25:00.093-03" 
  
}

````


|   Caso       | Status |      Respuesta         |
| :-------:    | :----: | :---------------------:|
|   Exito      |  200   | {"offer created" }     |
| Unauthorized |  401   |                        |
|   Fallo      |  500   |   { "Server error" }   |

#### 9 (OfferPost)

####


#### 1 Agregar a una oferta existente un post


  ##### `Post /api/OfferPost/create`

Solo se puede agregar a las ofertas productos propios, no importa si es admin o no.


  En el body de la request


```json

{
    "offer_post_postId":7,
    "offer_post_offerId" :1003,
    "offer_post_discount" :10
}

````


|   Caso       | Status |      Respuesta                                        |
| :-------:    | :----: | :---------------------:                               |
|   Exito      |  200   | {"offer created" }                                    |
| Unauthorized |  401   |                                                       |
| Conflict     |  409   |{"This publication already belongs to an active offer"}|
| BadRequest   |  400   |{"Only active posts can be added to offers"}           |
|   Fallo      |  500   |   { "Server error" }                                  |