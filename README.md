# Extrados Store API

<div style="text-align: justify">

<hr/>

## Introducci�n

**Extrados Store** es una API De ecommerce donde los usuarios pueden registrarse, vender sus productos o comprar productos publicados por otros usuarios.



### Estructura del Proyecto/Capas

El proyecto sigue una estructura, dividida en varias capas:

#### Capa Config

En esta capa se encuentran las configuraciones del proyecto, incluyendo información sensible necesaria para JWT y la conexión a la base de datos.

#### Capa Common

La capa common es el lugar donde se almacena cosas que podemos requerir en cualquier lugar del proyecto  Aquí se incluyen:

- Custom Exceptions: Manejo de excepciones personalizadas.
- Request: Definición de las estructuras de las solicitudes HTTP y sus validaciones.
- Otros elementos compartidos necesarios en múltiples capas.

#### Capa Entities

En esta capa, se definen los modelos y DTOs. Aquí se encuentran las representaciones de los datos y los objetos de transferencia de datos utilizados en todo el proyecto.

#### Capa Services

La capa Services contiene la lógica de negocio de la aplicación. Aquí se implementan los servicios que son consumidos por la capa API.

#### Capa API

La capa API actúa como interfaz de usuario y consume los servicios proporcionados por la capa Services. Aquí se gestionan las solicitudes HTTP, la autenticación con JWT, y la interacción con los servicios.

#### Capa DAOs

La capa DAOs  se encarga de la interacción directa con la base de datos. Aquí se implementan las operaciones CRUD y se gestionan las consultas a la base de datos.




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

Las clases que maneja Extrados Store son las siguientes:

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
|   post_img               | string                |
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

|  Nombre                 |  Tipo          |
| :-----------------:     | :----:         |
|  offer_id               | int pk         |
|  offer_name             | string         |
|  offer_date_start       | long           |
|  offer_date_expiration  | long           |
|  offer_status           | bool           |
|  offer_userId           | int(fk_user)   |

### 9. OfferPost

**Atributos**

|  Nombre              |  Tipo        |
| :-----------------:  | :----:       |
| offer_post_id        | int pk       |
| offer_post_postId    | int fk_post  |
| offer_post_offerId   | int fk_offer |
| offer_post_discount  | int          |
| offer_post_status    | bool         |

### 10. Car

**Atributos**

|  Nombre   |  Tipo        |
| :------:  | :----:       |
| car_id    | int pk       |
| user_id   | int fk_user  |
| post_id   | int fk_post  |
| quantity  | int          |




<hr />


## Endpoints 

La API cuenta con 11 tipos de endpoints:

- **Auth**: Iniciar sesión, tokens y registrarse.

- **Role**: Acciones relacionadas con los roles (crear y obtener información).

- **User**: Operaciones CRUD del usuario (algunas solo se pueden acceder siendo admin).

- **Category**: Crear categorías para las publicaciones.

- **Brand**: Crear marcas para las publicaciones.

- **Post**: CRUD de publicaciones y modificar el status.

- **PostStatus**: CRUD de la tabla estados para las publicaciones (active, paused, cancelled).

- **Offer**: CRUD de la tabla offer; las ofertas tienen una fecha de inicio y fin, las publicaciones pueden o no estar ligadas a una oferta.

- **OfferPost**: Este controlador une las publicaciones con las ofertas.

- **PostSearch**: Todo lo relacionado con buscar post y ofertas está en este controlador.

- **PurchaseHistory**: Todo lo relacionado con el historial de compras del usuario.

- **Car**: Todo lo relacionado con las compras del usuario.

Casi todos los endpoints necesitan un token en la cabecera para funcionar; de no recibirlo se obtendrá un Unauthorized (401). Algunos son solo para el rol admin; esto se verifica con el token. De no ser admin, se devolverá un Forbidden (403).




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

|   Caso    | Status |             Respuesta                              |
| :-------: | :----: | :--------------------------------:                 |
|   Exito   |  200   |          { "succes" }                              |
| Not Found |  404   |     { "*user* role not found" }                    |
| Conflict  |  409   | { "The phone number is already in use" }           |
| Conflict  |  409   |      { "The email is already in use" }             |
|   Fallo   |  500   | { "Something went wrong. Please contact support" } |


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

|   Caso       | Status |             Respuesta                             |
| :-------:    | :----: | :--------------------------------:                |
|   Exito      |  200   |    { refresh y access token }                     |
|   NotFound   |  404   |    { user not found }                             |
| Unauthorized |  401   |     { "incorrect password" }                      |
| Unauthorized |  401   |    { "The user is disabled" }                     |
|   Fallo      |  500   | { "Something went wrong. Please contact support" }|


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

|   Caso       | Status |             Respuesta                             |
| :-------:    | :----: | :--------------------------------:                |
|   Exito      |  200   |    { refresh y access token }                     |
| Unauthorized |  401   |     { " invalid token" }                          |
| Unauthorized |  401   |    { "invalid refresh token" }                    |
| Unauthorized |  401   |    { "invalid access token" }                     |
| Unauthorized |  401   | { "Invalid access token format" }                 |
|   Fallo      |  500   | { "Something went wrong. Please contact support" }|


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

|   Caso       | Status |             Respuesta                            |
| :-------:    | :----: | :--------------------------------:               |
|   Exito      |  200   |              { "succes"  }                       |
|   Forbidden  |  403   |                                                  |
| Unauthorized |  401   |                                                  |
| Conflict     |  409   | { " The name role is already in use" }           |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|  


#### 2 Obtener todos los roles y la info. Requiere rol admin

##### `GET /api/Role/getroles`



##### Respuestas

|   Caso       | Status |             Respuesta                            |
| :-------:    | :----: | :--------------------------------:               |
|   Exito      |  200   |        { lista de roles  }                       |
| Unauthorized |  401   |                                                  |
|   Forbidden  |  403   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|

Devuelve los roles


#### 3 (User)

#### 1 Deshabilita un usuario. Requiere rol admin

   ##### `PUT /api/User/disable/userId`


|   Caso       | Status |      Respuesta                                    |
| :-------:    | :----: | :---------------------:                           |
|   Exito      |  200   |   { "user disable"  }                             |
| Unauthorized |  401   |                                                   |
|   Forbidden  |  403   |                                                   |
|   Not Found  |  404   |   {"user not found"}                              |
| Conflict     |  409   |{"the user was already disable"}                   |
|   Fallo      |  500   | { "Something went wrong. Please contact support" }|

Caso exitoso cambia el status del usuario a false

#### 2 Habilita un usuario. Requiere rol admin

   ##### `PUT /api/User/enable/userId`


|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   |   { "user enable"  }                             |
| Unauthorized |  401   |                                                  |
|   Forbidden  |  403   |                                                  |
|   Not Found  |  404   |   {"user not found"}                             |
| Conflict     |  409   |{"the user was already enable"}                   | 
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|

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
|   Fallo      |  500   | { "Something went wrong. Please contact support" }   |

Caso exitoso cambia el status del usuario a true



#### 4 Obtener todos los usuarios (con el nombre de su rol y su status correspondiente)

   ##### `Get /api/User/getusers`
Solo el admind tiene acceso a este endpoint


|   Caso       | Status |      Respuesta                                    |
| :-------:    | :----: | :---------------------:                           |
|   Exito      |  200   |   { "lista de usuarios"  }                        | 
|   Fallo      |  500   | { "Something went wrong. Please contact support" }|

Caso exitoso traer todos los usuarios




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

|   Caso       | Status |      Respuesta                                    |
| :-------:    | :----: | :---------------------------------:               |
|   Exito      |  200   | { "category created" }                            |
| Unauthorized |  401   |                                                   |
|   Forbidden  |  403   |                                                   |
|   Conflict   |  409   | {"name category is already in use"}               |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" } |


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
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }                                                |



 #### 3 Obtener la lista de categorias creadas
  
  #####  `GET /api/Category/getcategorys`

|   Caso       | Status |     Respuesta                                    |
| :-------:    | :----: | :----------------------:                         |
|   Exito      |  200   | { [all categgorys] }                             |
| Unauthorized |  401   |                                                  |
|   Forbidden  |  403   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|


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

|   Caso       | Status |      Respuesta                                    |
| :-------:    | :----: | :---------------------------------:               |
|   Exito      |  200   | { "brand created" }                               |
| Unauthorized |  401   |                                                   |
|   Forbidden  |  403   |                                                   |
|   Conflict   |  409   | {"the name brand is already in use"}              |
|   Fallo      |  500   | { "Something went wrong. Please contact support" }|


#### 2 Obtener array de todos los brand 

  ##### `GET /api/Brand/getbrands`

|   Caso       | Status |     Respuesta                                     |
| :-------:    | :----: | :-------------------:                             |
|   Exito      |  200   | { [all brands] }                                  |
| Unauthorized |  401   |                                                   |
|   Forbidden  |  403   |                                                   |
|   Fallo      |  500   | { "Something went wrong. Please contact support" }|



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
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }            |


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

|   Caso       | Status |      Respuesta                                     |
| :-------:    | :----: | :---------------------------------:                |
|   Exito      |  200   | { "post  created" }                                |
| Unauthorized |  401   |                                                    |
|   Fallo      |  500   | { "Something went wrong. Please contact support" } |



#### 2 Pausar Una publicacion

  ##### `Put /api/Post/statuspaused/(postId)` 

Los clientes con rol usuario solo podran pausar sus propios post, el admin puede pausar cualquiera

 



|   Caso       | Status |      Respuesta                                      |
| :-------:    | :----: | :---------------------------------:                 |
|   Exito      |  200   | { "brand created" }                                 |
| Unauthorized |  401   |                                                     |
|   Forbidden  |  403   |                                                     |
| NotFound     |  404   | { "status not found"}                               |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }|


#### 3 Cancelar Una publicacion

  ##### `Put /api/Post/statuscancelled/(postId)` 

Solo el admin puede setear el estado de una publicacion en "cancelled" tabla post_status


|   Caso       | Status |      Respuesta                                      |
| :-------:    | :----: | :---------------------:                             |
|   Exito      |  200   | { "post status now is cancelled" }                  |
| Unauthorized |  401   |                                                     |
|   Forbidden  |  403   |                                                     |
| NotFound     |  404   | { "status not found"}                               |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }|


#### 4 Activar un post

  ##### `Put /api/Post/statusactive/(postId)/newStock` 

Los usuarios solo pueden activar sus propios post (incluyendo al admin) el admin no puede activar publicaciones de otros usuarios.
Tambien es necesario mandar en la url el nuevo stock (int), es decir que tambien actualiza el stock

|   Caso       | Status |      Respuesta                                      |
| :-------:    | :----: | :---------------------:                             |
|   Exito      |  200   | {"post status now is active" }                      |
| Unauthorized |  401   |                                                     |
| NotFound     |  404   | { "status not found"}                               |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }|


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


|   Caso       | Status |      Respuesta                                      |
| :-------:    | :----: | :---------------------:                             |
|   Exito      |  200   | {"post status now is active" }                      |
| Unauthorized |  401   |                                                     |
| NotFound     |  404   | { "status not found"}                               |
| BadRequest   |  400   | { "Post stock must have a valid positive value "}   |
| BadRequest   |  400   | { "Post price must have a valid positive value "}   |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }|

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


|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"offer created" }                               |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|

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
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }                            |

#### 2 Desligar un post de una oferta


  ##### `Delete /api/OfferPost/delete/(postId)`

Si el rol es admin, puede borrar cualquier post de cualquier oferta.
Si el rol es user , solo puede borrar sus publicaciones. Si un usuario intenta borrar el registro de otro usuario, recibira un 401



|   Caso       | Status |      Respuesta                                       |
| :-------:    | :----: | :---------------------:                              |
|   Exito      |  200   | {"offer created" }                                   |
| Unauthorized |  401   |                                                      |
| Notfound     |  404   |{"offer post not found"}                              |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }                   |

#### 3 Desligar todas las publicaciones de una oferta


  ##### `Delete /api/OfferPost/deleteall/(offerID)`

Solo el admin puede acceder a este endpoint, desliga todas las publicaciones ligadas a una oferta.

|   Caso       | Status |      Respuesta                                      |
| :-------:    | :----: | :---------------------:                             |
|   Exito      |  200   | {"All offer posts were deleted" }                   |
|   Forbidden  |  403   |                                                     |
| Unauthorized |  401   |                                                     |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }|


#### 10 (car)

####


#### 1 Agregar un producto al carrito de compras o aumentar el quantity en 1 a un producto que ya este en el carrito


  ##### `Post /api/Car/addtocar`

Agregar un producto, con status "active" y un stock mayor al quantity de la request body.
No se le resta el stock al producto. Solo se consulta. El stock al producto se resta cuando se efectua la compra.
Nadie puede comprar sus propias publicaciones

  En el body de la request


```json

{
    "post_id":8,
    "quantity":2
}

````


|   Caso       | Status |      Respuesta                                      |
| :-------:    | :----: | :---------------------:                             |
|   Exito      |  200   | {"offer created" }                                  |
| Unauthorized |  401   |                                                     |
| NotFound     |  404   |{"post not found"}                                   |
| NotFound     |  404   |{"post Status not found"}                            |
| Conflict     |  409   |{"stock is less than quantity"}                      |
| BadRequest   |  400   |{"a user cannot buy your posts"}                     |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }|


#### 2 Borrar un producto o disminuir en uno el quantity


  ##### `Post /api/Car/delete/(postId)`

Se reduce en un el quantity de un post o se elimina al llegar a 0.


|   Caso       | Status |      Respuesta                                      |
| :-------:    | :----: | :---------------------:                             |
|   Exito      |  200   | {"offer created" }                                  |
| Unauthorized |  401   |                                                     |
| NotFound     |  404   |{"post not found"}                                   |
|   Fallo      |  500   |   { "Something went wrong. Please contact support" }|


#### 3 Obtener el carrito del usuario logeado.


  ##### `Get /api/Car/getcar`

Se obtienen todos los post que el usuario agrego a su carrito, y ademas El costo total que saldría comprar ese carrito.
Teniendo en cuenta ofertas y cantidades de cada producto


|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista de post y costo total" }                 |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|

#### 4 Comprar el carrito del usuario logeado.


  ##### `Post /api/Car/buycar`

Se crean registros en la tabla sales y salesDetails. Equivalentes a los post que estaban en la tabla car.
Luego de esto se borran todos los registros del carrito. Para actualizarlo.


|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"success" }                                     |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|


#### 5 Obtener el carrito de cualquier usuario.


  ##### `Get /api/Car/getcar/(userId)`

se obtiene todos los post que el usuario almance en su carrito. Solo el admin tiene acceso a este enpoint


|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista de post y costo total" }                 |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|


#### 11 (PurchaseHistory)

####


#### 1 Obtener el registro de cada compra que realizo el usuario, con su fecha, costo y el producto.


  ##### `Get /api/PurchaseHistory/gethistory`

Se obtiene un registro de cada producto que compro, con una fecha (epoch).
Se informa precio y cantidad
El descuento ya esta integrado al precio final.
No existe una tabla historial de usuario, se obtiene la tabla sales_detail



|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista de compras" }                            |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|


#### 2 Obtener el registro de cada compra que realizo cualquier usuario, con su fecha, costo y el producto.


  ##### `Get /api/PurchaseHistory/gethistory/(userId)`

Solo el admin tiene acceso a este endpoint, se puede obtener el registro de compras de cualquier usuario.

|   Caso       | Status |      Respuesta                                    |
| :-------:    | :----: | :---------------------:                           |
|   Exito      |  200   | {"lista de compras" }                             |
| Unauthorized |  401   |                                                   |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" } |

#### 11 (PostSearch)

Este controlador contiene todos los endpoinst para buscar publicaciones, ya sea con o sin ofertas, con estado activo, pausado o cancelado.
Tiene un buscador para el admin, que permite ver todos los productos y filtrarlos por marca y categoría.
####


#### 1 Obtener todos las publicaciones activas, sin ofertas o con su oferta vencida.


  ##### `Get /api/PostSearch/getallactive`





|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista publicaciones" }                         |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|



#### 2 Obtener todas las publicaciones activas con ofertas tambien activas.


  ##### `Get /api/PostSearch/getallactivewithoffer`





|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista publicaciones con ofertas vigentes" }    |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|




#### 3 Obtener todas las publicaciones activas ligadas a una oferta activa


  ##### `Get /api/PostSearch/getallActivebyofferid/(offerId)`




|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista publicaciones ligadas a un oferta " }    |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|


#### 4 Obtener todas las publicaciones con ofertas vencidas y no vencidas

Este endpoint es solo para el admin, trae todas las publicaciones con ofertas, oferta vencidas o en vigencia.

  ##### `Get /api/PostSearch/getallwithoffer`


|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista publicaciones con ofertas vencida o no"} |
|   Forbidden  |  403   |                                                     |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|



#### 5 Obtener todas las publicaciones de un usuario, con y sin oferta.

Este endpoint es solo para el admin, trae todas las publicaciones (De cualquier usuario) con ofertas, vencidas o en vigencia.
No importa el status de la publicacion, pausado, cancelado o activo. Ademas trae un detalle del estado de la oferta y de la publicacion en si.

  ##### `Get /api/PostSearch/getallbyuser/(userId)`



|   Caso       | Status |      Respuesta                                   |
| :-------:    | :----: | :---------------------:                          |
|   Exito      |  200   | {"lista publicaciones detallada de un usuario"}  |
|   Forbidden  |  403   |                                                  |
| Unauthorized |  401   |                                                  |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }|

#### 6  filtro de busquedad por categoría, marca, y por nombre. No toma en cuenta ofertas (post con status active).

(ESTE ENDPOINT ESTA A MODO DE PRUEBA, HAY QUE MODIFICARLO)

Este endpoint es solo para el admin, filtra productos por un id de categoría, por un id de marca y por nombre.
Es decir busca publicaciones que contengan el string ingresado en el body de la request.
Ninguno de los campos son obligatorios, se puede filtrar solo por marca o solo por nombre por ejemplo.
solo trae publicaciones con post_statusId 1, cosa que esta mal. 

  ##### `Post /api/PostSearch/searchpost`


    En el body de la request


```json

{
    "postName":"tomo 1 one piece",
    "postCategoryId":1,
    "postBrandId" :1
}

````


|   Caso       | Status |      Respuesta                                     |
| :-------:    | :----: | :---------------------:                            |
|   Exito      |  200   |{"lista publicaciones segun los filtros ingresados"}|
|   Forbidden  |  403   |                                                    |
| Unauthorized |  401   |                                                    |
|   Fallo      |  500   |{ "Something went wrong. Please contact support" }  |
