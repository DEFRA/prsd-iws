GRANT SELECT, INSERT, UPDATE, DELETE, EXEC ON SCHEMA::[Notification] TO [iws_application]
GRANT SELECT, INSERT, UPDATE, DELETE, EXEC ON SCHEMA::[ImportNotification] TO [iws_application]
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::[Identity] TO [iws_application]
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::[Person] TO [iws_application]
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::[FileStore] TO [iws_application]
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::[Draft] TO [iws_application]
GRANT SELECT, EXEC ON SCHEMA::[Reports] TO [iws_application]
GRANT SELECT, EXEC ON SCHEMA::[Search] TO [iws_application]

GRANT SELECT, INSERT ON SCHEMA::[Auditing] TO [iws_application]

GRANT SELECT ON SCHEMA::[Lookup] TO [iws_application]

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::[HangFire] TO [iws_application]

GO