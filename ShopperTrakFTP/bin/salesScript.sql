SET NOCOUNT ON;
SELECT s.str_id as StoreIdentification, 
       Replace(Cast(CONVERT(DATE, s.bus_dt, 112) AS VARCHAR(max)), '-', '') as TransactionDate, 
       Replace(CONVERT (VARCHAR(8), tran_end_dttm, 108), ':', '') as TransactionEndTime, 
       Cast(winr_net_sale_amt AS MONEY) as Sales, 
       s.tran_id as TransactionID

FROM   winretailvapor.dbo.sales s(nolock) 
       INNER JOIN winretailvapor.dbo.sales_detail sd(nolock) 
               ON s.str_id = sd.str_id 
                  AND s.rgst_id = sd.rgst_id 
                  AND s.bus_dt = sd.bus_dt 
                  AND s.tran_id = sd.tran_id 
				  where s.bus_dt = CAST(GETDATE()-665 AS date)  --need to change to getDate() - 1 when implemented