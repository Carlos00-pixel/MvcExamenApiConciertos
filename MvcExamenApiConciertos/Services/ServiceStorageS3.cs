﻿using Amazon.S3;
using Amazon.S3.Model;
using MvcExamenApiConciertos.Models;

namespace MvcExamenApiConciertos.Services
{
    public class ServiceStorageS3
    {
        private string BucketName;
        private IAmazonS3 ClientS3;
        public ServiceStorageS3(IConfiguration configuration, IAmazonS3 clientS3, KeysModel model)
        {
            this.BucketName = model.BucketName;
            this.ClientS3 = clientS3;
        }

        //COMENZAMOS SUBIENDO FICHEROS AL BUCKET
        //NECESITAMOS FileName, Stream y un Key/Value
        public async Task<bool>
            UploadFileAsync(string fileName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = this.BucketName
            };
            //DEBEMOS OBTENER UNA RESPUESTA CON EL MISMO TIPO 
            //DE REQUEST
            PutObjectResponse response = await
                this.ClientS3.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
