﻿using API_Server.Models;
using API_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API_Server.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;

        public DocumentController(DocumentService documentService)
        {
            _documentService = documentService;
        }

        // API to upload a document
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] Document document, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var documentId = await _documentService.UploadDocumentAsync(document, stream);
                    return Ok(new { DocumentId = documentId, UploaderName = document.UploaderName });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading document: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentDetails(string id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }

        // API to get documents uploaded by the current user
        [Authorize]
        [HttpGet("mydocuments")]
        public async Task<IActionResult> GetMyDocuments([FromQuery] string username)
        {
            var documents = await _documentService.GetDocumentsByUploaderAsync(username);
            return Ok(documents);
        }

        // API to download a document
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadDocument(string id, [FromQuery] string username)
        {
            if (await _documentService.HasAccessAsync(id, username))
            {
                var stream = await _documentService.DownloadDocumentAsync(id);
                if (stream == null)
                {
                    return NotFound();
                }

                return File(stream, "application/octet-stream", id);
            }
            else
            {
                return Forbid();
            }
        }

        // API to get documents downloaded by the current user
        [Authorize]
        [HttpGet("mydownloads")]
        public async Task<IActionResult> GetMyDownloads([FromQuery] string username)
        {
            var documents = await _documentService.GetDownloadedDocumentsAsync(username);
            return Ok(documents);
        }

        // API to edit a document
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDocument(string id, [FromBody] Document updatedDocument)
        {
            if (updatedDocument == null || string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid document information.");
            }

            await _documentService.UpdateDocumentAsync(id, updatedDocument);
            return Ok("Document updated successfully.");
        }

        // API to delete a document
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return Ok("Document deleted successfully.");
        }

        // API to search documents
        [HttpGet("search")]
        public async Task<IActionResult> SearchDocuments([FromQuery] string keyword, [FromQuery] string username, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var (documents, totalPages) = await _documentService.SearchDocumentsAsync(keyword, username, pageNumber, pageSize);
            Response.Headers.Add("X-Total-Pages", totalPages.ToString());
            return Ok(documents);
        }

        // API to view content
        [HttpGet("content/{id}")]
        public async Task<IActionResult> GetDocumentContent(string id)
        {
            try
            {
                var content = await _documentService.DownloadDocumentContentAsync(id);
                if (content == null)
                {
                    return NotFound();
                }
                return File(content, "application/octet-stream", id);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}