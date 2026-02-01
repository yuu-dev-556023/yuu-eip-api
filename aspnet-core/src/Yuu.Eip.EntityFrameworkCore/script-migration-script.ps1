cd .\

# ScriptMigration.ps1

# 啟用錯誤終止
$ErrorActionPreference = "Stop"

try {
    # 提示使用者輸入 From 遷移名稱，如果沒有輸入則設為 "0"
    $FromMigrationName = Read-Host "請輸入 From 遷移名稱 (MigrationName)，若不輸入則預設為 0"
    if ([string]::IsNullOrWhiteSpace($FromMigrationName)) {
        $FromMigrationName = "0"  # 若沒輸入，則設定為 "0"
    }

    # 提示使用者輸入 To 遷移名稱
    $ToMigrationName = Read-Host "請輸入 To 遷移名稱 (MigrationName)，若不輸入則預設為 最新的遷移"

    # 顯示選項清單
    # Write-Host "請選擇要使用的Context：" -ForegroundColor Yellow
    # Write-Host "1. CeosHrDbContext" -ForegroundColor Cyan
    # Write-Host "2. ReportDataDbContext" -ForegroundColor Cyan

    # 提示使用者輸入選項
    # $choice = Read-Host "輸入數字選擇"

    # 根據使用者輸入的選項執行操作
    # switch ($choice) {
    #     "1" {
    #         $ContextName = "CeosHrDbContext"
    #     }
    #     "2" {
    #         $ContextName = "ReportDataDbContext"
    #     }
    #     default {
    #         Write-Host "無效的選擇，請選擇 1 或 2" -ForegroundColor Red
    #     }
    # }

    # 提示使用者輸入輸出路徑（可省略-預覽指令）
    $OutputFileName = Read-Host "請輸入檔案名稱，沒輸入則預覽指令"

    # 構建 dotnet ef migrations script 指令
    $Command = "dotnet ef migrations script $FromMigrationName $ToMigrationName"

    # ContextName 有值才加
    if (-not [string]::IsNullOrWhiteSpace($ContextName)) {
        $Command += " -c $ContextName -v"
    }

    # 如果有輸入輸出路徑，則將 -o 參數加入指令
    if (![string]::IsNullOrWhiteSpace($OutputFileName)) {
        $Command += " -o DatabaseScripts\$OutputFileName.sql"
    }

    $Command += " -- local-dev skip"

    # 顯示將要執行的指令
    Write-Host "`n將執行以下指令：" -ForegroundColor Yellow
    Write-Host $Command -ForegroundColor Cyan

    # 執行指令
    Invoke-Expression $Command

    Write-Host "`n指令輸出成功！" -ForegroundColor Green
}
catch {
    Write-Host "`n發生錯誤：" $_.Exception.Message -ForegroundColor Red
}

Pause
