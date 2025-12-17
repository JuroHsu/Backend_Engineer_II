using BackendExamHub.Models.Database;
using BackendExamHub.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendExamHub.Services;

public class MyofficeServices(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found");
    public async Task<AccountResponse?> GetAccountById(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT * FROM MyOffice_ACPD WHERE ACPD_SID = @Sid",
            connection);

        command.Parameters.AddWithValue("@Sid", id);
        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
            return MapToAccountResponse(reader);

        return null;
    }

    public async Task<List<AccountResponse>> GetAccounts()
    {
        var accounts = new List<AccountResponse>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT * FROM MyOffice_ACPD ORDER BY ACPD_UPDDateTime DESC",
            connection);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            accounts.Add(MapToAccountResponse(reader));
        }

        return accounts;
    }
    public async Task CreateAccount(CreateAccountRequest request)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // 使用輔助方法產生 SID
            var newSid = await GenerateNewSidAsync("MyOffice_ACPD", connection);

            using var command = new SqlCommand(
                @"INSERT INTO MyOffice_ACPD 
                (ACPD_SID, ACPD_Cname, ACPD_Ename, ACPD_Sname, ACPD_Email, 
                ACPD_Status, ACPD_Stop, ACPD_StopMemo, ACPD_LoginID, ACPD_LoginPWD, 
                ACPD_Memo, ACPD_NowID, ACPD_UPDID)
                VALUES 
                (@SID, @Cname, @Ename, @Sname, @Email, 
                @Status, @Stop, @StopMemo, @LoginID, @LoginPWD, 
                @Memo, @NowID, @UPDID)",
                connection);

            command.Parameters.AddWithValue("@SID", newSid);
            command.Parameters.AddWithValue("@Cname", request.ACPD_Cname);
            command.Parameters.AddWithValue("@Ename", request.ACPD_Ename ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Sname", request.ACPD_Sname ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", request.ACPD_Email);
            command.Parameters.AddWithValue("@Status", request.ACPD_Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Stop", request.ACPD_Stop ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@StopMemo", request.ACPD_StopMemo ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LoginID", request.ACPD_LoginID);

            // 密碼加密
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.ACPD_LoginPWD);
            command.Parameters.AddWithValue("@LoginPWD", hashedPassword);

            command.Parameters.AddWithValue("@Memo", request.ACPD_Memo ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@NowID", "SYSTEM");
            command.Parameters.AddWithValue("@UPDID", "SYSTEM");

            var rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
                throw new InvalidOperationException("Failed to create account");
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            throw new InvalidOperationException($"Account '{request.ACPD_LoginID}' already exists");
        }
    }
    public async Task UpdateAccount(UpdateAccountRequest request)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            // 如果有提供新密碼，才更新密碼
            string sql = $@"UPDATE MyOffice_ACPD 
                    SET ACPD_Cname = @Cname,
                        ACPD_Ename = @Ename,
                        ACPD_Sname = @Sname,
                        ACPD_Email = @Email,
                        ACPD_Status = @Status,
                        ACPD_Stop = @Stop,
                        ACPD_StopMemo = @StopMemo,
                        ACPD_LoginID = @LoginID,
                        {(string.IsNullOrWhiteSpace(request.ACPD_LoginPWD) ? "" : "ACPD_LoginPWD = @LoginPWD,")}
                        ACPD_Memo = @Memo,
                        ACPD_UPDDateTime = @UPDDateTime,
                        ACPD_UPDID = @UPDID
                    WHERE ACPD_SID = @SID";


            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@SID", request.ACPD_SID);
            command.Parameters.AddWithValue("@Cname", request.ACPD_Cname);
            command.Parameters.AddWithValue("@Ename", request.ACPD_Ename ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Sname", request.ACPD_Sname ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", request.ACPD_Email);
            command.Parameters.AddWithValue("@Status", request.ACPD_Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Stop", request.ACPD_Stop ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@StopMemo", request.ACPD_StopMemo ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LoginID", request.ACPD_LoginID);

            if (!string.IsNullOrWhiteSpace(request.ACPD_LoginPWD))
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.ACPD_LoginPWD);
                command.Parameters.AddWithValue("@LoginPWD", hashedPassword);
            }

            command.Parameters.AddWithValue("@Memo", request.ACPD_Memo ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@UPDDateTime", DateTime.Now);
            command.Parameters.AddWithValue("@UPDID", "SYSTEM");

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
                throw new KeyNotFoundException($"Account with ID '{request.ACPD_SID}' not found");
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            throw new InvalidOperationException($"Account '{request.ACPD_LoginID}' already exists");
        }
    }

    public async Task DeleteAccount(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID is required");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "DELETE FROM MyOffice_ACPD WHERE ACPD_SID = @Sid",
                connection);

            command.Parameters.AddWithValue("@Sid", id);

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"Account with ID '{id}' not found");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAccounts(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
            throw new ArgumentException("No account IDs provided");

        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                var parameters = string.Join(",", ids.Select((id, index) => $"@Id{index}"));
                using var command = new SqlCommand(
                    $"DELETE FROM MyOffice_ACPD WHERE ACPD_SID IN ({parameters})",
                    connection,
                    transaction);

                for (int i = 0; i < ids.Count; i++)
                {
                    command.Parameters.AddWithValue($"@Id{i}", ids[i]);
                }

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                    throw new InvalidOperationException("No accounts were deleted");

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #region Helper Methods
    private static AccountResponse MapToAccountResponse(SqlDataReader reader)
    {
        return new AccountResponse
        {
            ACPD_SID = reader["ACPD_SID"]?.ToString() ?? string.Empty,
            ACPD_Cname = reader["ACPD_Cname"] as string,
            ACPD_Ename = reader["ACPD_Ename"] as string,
            ACPD_Sname = reader["ACPD_Sname"] as string,
            ACPD_Email = reader["ACPD_Email"] as string,
            ACPD_Status = reader["ACPD_Status"] != DBNull.Value
                ? Convert.ToByte(reader["ACPD_Status"])
                : null,
            ACPD_Stop = reader["ACPD_Stop"] != DBNull.Value
                ? Convert.ToBoolean(reader["ACPD_Stop"])
                : null,
            ACPD_StopMemo = reader["ACPD_StopMemo"] as string,
            ACPD_LoginID = reader["ACPD_LoginID"] as string,
            // 密碼不回傳
            ACPD_Memo = reader["ACPD_Memo"] as string,
            ACPD_NowDateTime = reader["ACPD_NowDateTime"] != DBNull.Value
                ? Convert.ToDateTime(reader["ACPD_NowDateTime"])
                : null,
            ACPD_NowID = reader["ACPD_NowID"] as string,
            ACPD_UPDDateTime = reader["ACPD_UPDDateTime"] != DBNull.Value
                ? Convert.ToDateTime(reader["ACPD_UPDDateTime"])
                : null,
            ACPD_UPDID = reader["ACPD_UPDID"] as string
        };
    }
    /// <summary>
    /// 呼叫 NEWSID stored procedure 產生唯一的 SID
    /// </summary>
    /// <param name="tableName">資料表名稱</param>
    /// <param name="connection">資料庫連線</param>
    /// <returns>產生的 SID (20 字元)</returns>
    private static async Task<string> GenerateNewSidAsync(string tableName, SqlConnection connection)
    {
        using var command = new SqlCommand("NEWSID", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@TableName", tableName);

        var sidParam = new SqlParameter("@ReturnSID", SqlDbType.NVarChar, 20)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(sidParam);

        await command.ExecuteNonQueryAsync();

        return sidParam.Value?.ToString() ?? throw new InvalidOperationException($"Failed to generate SID for table {tableName}");
    }
    #endregion

}