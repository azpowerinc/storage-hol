// TODO: 1. NuGetからAzure.Storage.Blobsを追加
// TODO: 2.　Storage SDKのusingを追加

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

// TODO: 4. Storage Accountの接続文字列を取得し、環境変数に追加 (setx AZURE_STORAGE_CONNECTION_STRING "<yourconnectionstring>")
// TODO: 5. 環境変数から接続文字列を取得 (var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");)

var connectionString = "DefaultEndpointsProtocol=https;AccountName=apstoragedemo;AccountKey=p38haogOUm6/7q5m2+avS3FuCCzzl1gLLzS/fQkAGcjN9vc9PcxSB0BLtgzO4KkfxdxSOLKxJWWs+RTtmcoeuA==;EndpointSuffix=core.windows.net";

// TODO: 6. BlobServiceClientを作成し、コンテナを作成

var blobServiceClient = new BlobServiceClient(connectionString);
string containerName = "apfiles";

// pattern1:
// var containerClient = (await blobServiceClient.CreateBlobContainerAsync(containerName)).Value;

// pattern2:
var containerClient = new BlobContainerClient(connectionString, containerName);
await containerClient.CreateIfNotExistsAsync();

// TODO: 7. イメージファイルのプロパティ「出力ディレクトリにコピー」を「常にコピー」に変更し、BlobにデータをUpload

var uploadFilename = "fuji.jpg";
var blob = containerClient.GetBlobClient(uploadFilename);
using var fs = new FileStream(uploadFilename, FileMode.Open);


await blob.UploadAsync(fs);

// TODO 8. ContentType (MIME)を追加
var headers = new BlobHttpHeaders()
{
    ContentType = "image/jpeg",
};
blob.SetHttpHeaders(headers);

// TODO 9. アクセスレベルを変更

await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

// TODO: 10. コンテナの一覧を表示

await foreach (var blobItem in containerClient.GetBlobsAsync())
    Console.WriteLine($"\t{blobItem.Name}");

// TODO: 11. ダウンロード

var downloadDirectory = "downloads";
if (Directory.Exists(downloadDirectory) == false)
    Directory.CreateDirectory(downloadDirectory);
await blob.DownloadToAsync($"{downloadDirectory}\\{blob.Name}");
