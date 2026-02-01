cd .\

# UpdateDatabase.ps1

# 啟用錯誤終止
$ErrorActionPreference = "Stop"

try {
    # 提示使用者輸入遷移名稱
    $MigrationName = Read-Host "請輸入遷移名稱 (MigrationName)"

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

    # 顯示選項清單
    # Write-Host "請選擇要使用的Environment：" -ForegroundColor Yellow
    # Write-Host "1. local-dev" -ForegroundColor Cyan

    # 提示使用者選擇環境
    # $choice = Read-Host "輸入數字選擇"

    # 根據使用者輸入的選項執行操作
    $Environment = "local-dev"
    # switch ($choice) {
    #     "1" {
    #         $Environment = "local-dev"
    #     }
    #     default {
    #         Write-Host "無效的選擇" -ForegroundColor Red
    #     }
    # }

    # 構建 dotnet ef database update 指令
    $Command = "dotnet ef database update $MigrationName"

    # ContextName 有值才加
    if (-not [string]::IsNullOrWhiteSpace($ContextName)) {
        $Command += " -c $ContextName"
    }

    # Environment 有值才加（包含 -v）
    if (-not [string]::IsNullOrWhiteSpace($Environment)) {
        $Command += " -v -- $Environment"
    }

    # 顯示將要執行的指令
    Write-Host "`n將執行以下指令：" -ForegroundColor Yellow
    Write-Host $Command -ForegroundColor Cyan

    # 執行指令
    Invoke-Expression $Command

    Write-Host "`n資料庫更新成功！" -ForegroundColor Green
}
catch {
    Write-Host "`n發生錯誤：" $_.Exception.Message -ForegroundColor Red
}

# Pause
